import { ProjectService } from './../../shared/project/project.service';
import { Component, OnInit } from '@angular/core';
import { WorkItemService } from '../../shared/work-item/work-item.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styles: []
})

export class ProjectDetailComponent implements OnInit {
  projectId: number;
  constructor(private workItemService: WorkItemService, private activatedRoute: ActivatedRoute, private projectService: ProjectService) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.projectId = +params.get('id');
    });
  }
}
