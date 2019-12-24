import { UserService } from './../../shared/user/user.service';
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
  constructor(private workItemService: WorkItemService, private activatedRoute: ActivatedRoute, protected userService: UserService) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.projectId = +params.get('id');
    });
  }
}
