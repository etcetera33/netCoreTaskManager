import { PopupService } from './../../../services/popup.service';
import { NgForm } from '@angular/forms';
import { UserService } from './../../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkItemService } from './../../../services/work-item.service';
import { User } from './../../../models/user';
import { WorkItem } from './../../../models/work-item.model';
import { Component, OnInit } from '@angular/core';
import { ImageService } from 'src/app/services/image.service';
import { File as FileEntity } from 'src/app/models/file';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-create-work-item',
  templateUrl: './create-work-item.component.html',
  styles: []
})
export class CreateWorkItemComponent implements OnInit {
  projectId: number;
  workItem: WorkItem;
  assigneeList: User[];
  workItemTypes: any[];
  workItemStatuses: any[];
  role: string;
  urls: any[];
  files: FormData;
  filesEntity: FileEntity[];
  constructor(
    private workItemService: WorkItemService, private activatedRoute: ActivatedRoute,
    protected userService: UserService, private router: Router, private popupService: PopupService,
    protected imageService: ImageService,
    ) { }

  ngOnInit() {
    this.workItem = new WorkItem();
    this.files = new FormData();
    this.activatedRoute.paramMap.subscribe(params => {
      this.projectId = +params.get('id');
    });
    this.loadAssigneeList();
    this.loadWorkItemTypes();
    this.loadWorkItemStatuses();
    this.role = this.userService.getCurrentRole();
  }

  onSubmit(form: NgForm) {
    var data = JSON.stringify(form.value);
    this.workItemService.createEntity(data).subscribe(
      res => {
        this.router.navigate(['/projects/' + this.projectId]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadEntity(entityId: number) {
    this.workItemService.loadEntity(entityId).subscribe(
      res => {
        this.workItem = res as WorkItem;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

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

  onSelectFile(files) {
    if (files.length === 0) {
      return;
    }
    Array.from(files).forEach(file => {
      const fileToUpload = file as File; 
      this.files.append('file', fileToUpload);
    });
    this.saveFiles();
  }

  saveFiles() {
    console.log('saveChanges');
    this.imageService.createImage(this.files).subscribe(
    res => {
      this.workItem.Files = res as FileEntity[];
    },
    err => {
      console.log(err);
      this.popupService.openModal('error', err);
    });
  }
}
