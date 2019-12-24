import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
import { Router } from '@angular/router';
import { ProjectService } from './../../shared/project/project.service';
import { Component, OnInit } from '@angular/core';
import { NgForm  } from '@angular/forms';
import { Project } from '../../shared/project/project.model';
import { User } from '../../shared/user/user';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styles: []
})

export class CreateProjectComponent implements OnInit {
  project: Project;
  user: Promise<User>;
  role: string;
  constructor(
    protected projectService: ProjectService, private router: Router,
    private userService: UserService, private popupService: PopupService
    ) { }

  ngOnInit() {
    this.user = this.userService.getCurrentUser();
    this.user.then(res => this.role = res.Role);
    console.log(this.role);
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
