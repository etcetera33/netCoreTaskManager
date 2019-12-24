import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
import { Router } from '@angular/router';
import { ProjectService } from './../../shared/project/project.service';
import { Component, OnInit } from '@angular/core';
import { NgForm  } from '@angular/forms';
import { Project } from '../../shared/project/project.model';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styles: []
})

export class CreateProjectComponent implements OnInit {
  project: Project;
  constructor(
    protected projectService: ProjectService, private router: Router,
    protected userService: UserService, private popupService: PopupService
    ) { }

  ngOnInit() {}

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
