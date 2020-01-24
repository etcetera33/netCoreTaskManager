import { PopupService } from './../../../services/popup.service';
import { Router } from '@angular/router';
import { ImageService } from './../../../services/image.service';
import { Component, OnInit, Input } from '@angular/core';
import { File } from './../../../models/file';

@Component({
  selector: 'app-image-list',
  templateUrl: './image-list.component.html',
  styles: []
})
export class ImageListComponent implements OnInit {
  private images: File;
  private attachedImages: File[];
  currentPage: number;
  pagesCount: number;
  selectedImages: File[] = [];
  selectedToDeleteImage: File;
  @Input() entityId: number;
  constructor(private imageService: ImageService, private router: Router, private popupService: PopupService) { }

  ngOnInit() {
    this.loadImages();
    this.getAttached();
  }

  paginate(page: number) {
    this.currentPage = page;
    this.loadImages();
  }

  loadImages() {
    this.imageService.getImages(this.currentPage).subscribe(
      res => {
        this.pagesCount = +res.PagesCount;
        this.images = res.EntityList as File;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  onClick(image: File) {
    const index: number = this.selectedImages.indexOf(image);
    if (index !== -1) {
        this.selectedImages.splice(index, 1);
    } else {
      this.selectedImages.push(image);
    }

    console.log(this.getSelected());
  }

  isSelected(id: number) {
    return this.selectedImages.find(x => x.Id === id) === undefined ? false : true;
  }

  getSelected() {
    return this.selectedImages;
  }

  attachToWorkItem() {
    this.imageService.attachToWorkItem(this.selectedImages, this.entityId).subscribe(
      res => {
        this.router.navigate(['/projects']);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  removeFromAttachedToWorkItem() {
    this.imageService.removeFromAttachedToWorkItem(this.selectedToDeleteImage.Id, this.entityId).subscribe(
      () => {
        this.router.navigate(['/projects']);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  isAboutToDelete(image: File) {
    return image === this.selectedToDeleteImage;
  }

  selectedDeleteImageChanged(image: File) {
    this.selectedToDeleteImage = image;
  }

  getAttached() {
    this.imageService.getWorkItemImages(this.entityId).subscribe(
      res => {
        this.attachedImages = res as File[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
}
