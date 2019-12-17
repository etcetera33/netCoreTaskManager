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
  constructor(protected projectService: ProjectService, private router: Router) { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    console.log(form.value);
    this.projectService.createProject(form.value).subscribe(
      res => {
        this.router.navigate(['/projects']);
        window.location.href = '';
      },
      err => {
        console.log(err);
      }
    );
  }

}
