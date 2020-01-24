import { PopupService } from './../../../services/popup.service';
import { Router } from '@angular/router';
import { ImageService } from './../../../services/image.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-create-image',
  templateUrl: './create-image.component.html',
  styles: []
})

export class CreateImageComponent implements OnInit {
  files: FormData;
  constructor(protected imageService: ImageService, private router: Router, private popupService: PopupService) {}
  onSelectFile(files) {
    if (files.length === 0) {
      return;
    }
    Array.from(files).forEach(file => {
      const fileToUpload = file as File;
      this.files.append('file', fileToUpload, fileToUpload.name);
    });
  }

  onClick(form: NgForm) {
    this.imageService.createImage(this.files).subscribe(() =>
    () => {
      this.router.navigate(['/projects']);
    },
    err => {
      console.log(err);
      this.popupService.openModal('error', err);
    });
  }

  ngOnInit() {
    this.files = new FormData();
  }
}
