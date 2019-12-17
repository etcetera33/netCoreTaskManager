import { Project } from './../../shared/project/project.model';
import { Component, OnInit, Input } from '@angular/core';
import { ProjectService } from '../../shared/project/project.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styles: []
})
export class ProjectListComponent implements OnInit {
  projectList: Project[];
  constructor(protected projectService: ProjectService) { }

  ngOnInit() {
    this.projectService.getProjectList().subscribe(
      res => {
        this.projectList = res as Project[];
      },
      err => {
        console.log(err);
      }
    );
  }

}
