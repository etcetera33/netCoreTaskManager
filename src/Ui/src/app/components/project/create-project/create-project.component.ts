import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { Router } from '@angular/router';
import { ProjectService } from './../../../services/project.service';
import { Component, OnInit } from '@angular/core';
import { NgForm  } from '@angular/forms';
import { Project } from './../../../models/project.model';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styles: []
})

export class CreateProjectComponent implements OnInit {
  project: Project;
  role: string;
  constructor(
    protected projectService: ProjectService, private router: Router,
    private userService: UserService, private popupService: PopupService
    ) { }

  ngOnInit() {
    this.role = this.userService.getCurrentRole();
  }

  onSubmit(form: NgForm) {
    this.projectService.createProject(form.value).subscribe(
      res => {
        this.router.navigate(['/projects']);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
