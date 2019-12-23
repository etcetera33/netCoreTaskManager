import { NgForm } from '@angular/forms';
import { UserService } from './../../shared/user/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkItemService } from './../../shared/work-item/work-item.service';
import { User } from './../../shared/user/user';
import { WorkItem } from './../../shared/work-item/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-create-work-item',
  templateUrl: './create-work-item.component.html',
  styles: []
})
export class CreateWorkItemComponent implements OnInit {
  projectId: number;
  workItem: WorkItem;
  assigneeList: User[];
  workItemTypes: any[];
  workItemStatuses: any[];
  constructor(
    private workItemService: WorkItemService, private activatedRoute: ActivatedRoute,
    protected userService: UserService, private router: Router
    ) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      console.log(+params.get('id'));
      this.projectId = +params.get('id');
    });
    this.loadAssigneeList();
    this.loadWorkItemTypes();
    this.loadWorkItemStatuses();
  }

  onSubmit(form: NgForm) {
    const data = JSON.stringify(form.value);
    console.log(form.value);
    this.workItemService.createEntity(data).subscribe(
      res => {
        this.router.navigate(['/projects/' + this.projectId]);
      },
      err => {
        console.log(err);
      }
    );
  }

  loadEntity(entityId: number) {
    this.workItemService.loadEntity(entityId).subscribe(
      res => {
        this.workItem = res as WorkItem;
      },
      err => {
        console.log(err);
      }
    );
  }

  loadAssigneeList() {
    this.userService.getUserDictionary().subscribe(
      res => {
        this.assigneeList = res as User[];
      },
      err => {
        console.log(err);
      }
    );
  }

  loadWorkItemTypes() {
    this.workItemService.loadWorkItemTypes().subscribe(
      res => {
        this.workItemTypes = res as any[];
      },
      err => {
        console.log(err);
      }
    );
  }

  loadWorkItemStatuses() {
    this.workItemService.loadWorkItemStatuses().subscribe(
      res => {
        this.workItemStatuses = res as any[];
      },
      err => {
        console.log(err);
      }
    );
  }
}
