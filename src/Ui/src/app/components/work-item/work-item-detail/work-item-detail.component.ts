import { ImageService } from './../../../services/image.service';
import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { User } from './../../../models/user';
import { File as FileEntity } from './../../../models/file';
import { NgForm } from '@angular/forms';
import { WorkItemService } from './../../../services/work-item.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkItem } from './../../../models/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-work-item-detail',
  templateUrl: './work-item-detail.component.html',
  styles: []
})

export class WorkItemDetailComponent implements OnInit {
  urls = new Array<string>();
  workItem: WorkItem;
  assigneeList: User[];
  workItemTypes: any[];
  workItemStatuses: any[];
  role: string;
  images: File;
  attachedImages: FileEntity[] = [];
  currentPage: number;
  pagesCount: number;
  selectedImages: FileEntity[] = [];
  selectedToDeleteImage: FileEntity;
  files: FormData;
  constructor(
    private workItemService: WorkItemService, private activatedRoute: ActivatedRoute,
    protected userService: UserService, private router: Router, private popupService: PopupService, private imageService: ImageService
    ) { }

  ngOnInit() {
    this.files = new FormData();
    this.activatedRoute.paramMap.subscribe(params => {
      this.workItemService.setProjectId(+params.get('projectId'));
      this.loadEntity(+params.get('id'));
    });
    this.loadAssigneeList();
    this.loadWorkItemTypes();
    this.loadWorkItemStatuses();
    this.role = this.userService.getCurrentRole();
  }

  onSubmit(form: NgForm) {
    const data = JSON.stringify(form.value);
    this.workItemService.updateEntity(data, this.workItem.Id).subscribe(
      res => {
        this.router.navigate(['/projects/' + this.workItem.ProjectId]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  removeWorkItem() {
    const projectId = this.workItem.ProjectId;
    this.workItemService.removeItem(this.workItem.Id).subscribe(
      () => {
        this.router.navigate(['/projects/' + projectId]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  isSelected(id: number) {
    return this.selectedImages.find(x => x.Id === id) === undefined ? false : true;
  }

  attachToWorkItem() {
    this.imageService.attachToWorkItem(this.selectedImages, this.workItem.Id).subscribe(
      res => {
        this.router.navigate(['/projects']);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  /**
   *
   * Methods related to saving files
   *
   */
  onSelectFile(files) {
    if (files.length === 0) {
      return;
    }
    if (this.attachedImages !== null && this.attachedImages.length > 0) {
      this.urls = [];
      this.removeFiles();
    }
    if (files) {
      for (const file of files) {
        const fileToUpload = file as File;
        this.files.append('file', fileToUpload);
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.urls.push(e.target.result);
        };
        reader.readAsDataURL(file);
      }
    }
    this.saveFiles();
  }
  saveFiles() {
    this.imageService.createImage(this.files).subscribe(
    res => {
      this.attachedImages = res as FileEntity[];
    },
    err => {
      this.popupService.openModal('error', err);
    });
  }

  /**
   *
   * Methods related to deleting files
   *
   */
  removeFiles() {
    const data = JSON.stringify(this.attachedImages);
    this.imageService.remove(data).subscribe(
      () => {
        this.attachedImages = [];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
  removeFromAttached() {
    this.imageService.removeFromAttachedToWorkItem(this.selectedToDeleteImage.Id, this.workItem.Id).subscribe(
      () => {
        this.loadAttached();
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  selectedDeleteImageChanged(image: FileEntity) {
    this.selectedToDeleteImage = image;
  }

  /**
   *
   * On init methods
   *
   */
  loadAssigneeList() {
    this.userService.getUserDictionary().subscribe(
      res => {
        this.assigneeList = res as User[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
  loadWorkItemTypes() {
    this.workItemService.loadWorkItemTypes().subscribe(
      res => {
        this.workItemTypes = res as any[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
  loadWorkItemStatuses() {
    this.workItemService.loadWorkItemStatuses().subscribe(
      res => {
        this.workItemStatuses = res as any[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
  loadAttached() {
    this.workItemService.getAttached(this.workItem.Id).subscribe(
      res => {
        this.workItem.Files = res as FileEntity[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
  loadEntity(entityId: number) {
    this.workItemService.loadEntity(entityId).subscribe(
      res => {
        console.log('loading entity');
        this.workItem = res as WorkItem;
        this.loadAttached();
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
}
