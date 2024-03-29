import { PopupService } from './../../../services/popup.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from './../../../services/project.service';
import { Component, OnInit } from '@angular/core';
import { Project } from './../../../models/project.model';

@Component({
  selector: 'app-project-settings',
  templateUrl: './project-settings.component.html',
  styles: []
})
export class ProjectSettingsComponent implements OnInit {

  project: Project;
  constructor(
    private projectService: ProjectService, private route: ActivatedRoute,
    private router: Router, private popupService: PopupService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const projectId = +params.get('id');
      this.initializeEntity(projectId);
    });
  }

  initializeEntity(id: number) {
    this.projectService.getEntity(id).subscribe(
      res => {
        this.project = res as Project;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  update(form: NgForm) {
    const formData = JSON.stringify(form.value);

    this.projectService.updateProject(formData, this.project.Id).subscribe(
      () => {
        this.router.navigate(['/projects/' + this.project.Id]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
