import { UserService } from './../../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { WorkItemService } from './../../../services/work-item.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styles: []
})

export class ProjectDetailComponent implements OnInit {
  projectId: number;
  role: string;
  constructor(private workItemService: WorkItemService, private activatedRoute: ActivatedRoute, protected userService: UserService) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.projectId = +params.get('id');
    });
    this.role = this.userService.getCurrentRole();
  }
}
