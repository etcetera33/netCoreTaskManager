import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { User } from './../../../models/user';
import { NgForm } from '@angular/forms';
import { WorkItemService } from './../../../services/work-item.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkItem } from './../../../models/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-work-item-detail',
  templateUrl: './work-item-detail.component.html',
  styles: []
})

export class WorkItemDetailComponent implements OnInit {
  workItem: WorkItem;
  assigneeList: User[];
  workItemTypes: any[];
  workItemStatuses: any[];
  role: string;
  constructor(
    private workItemService: WorkItemService, private activatedRoute: ActivatedRoute,
    protected userService: UserService, private router: Router, private popupService: PopupService
    ) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.workItemService.setProjectId(+params.get('projectId'));
      this.loadEntity(+params.get('id'));
    });
    this.loadAssigneeList();
    this.loadWorkItemTypes();
    this.loadWorkItemStatuses();
    this.role = this.userService.getCurrentRole();
  }

  onSubmit(form: NgForm) {
    const data = JSON.stringify(form.value);
    this.workItemService.updateEntity(data, this.workItem.Id).subscribe(
      res => {
        this.router.navigate(['/projects/' + this.workItem.ProjectId]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadEntity(entityId: number) {
    this.workItemService.loadEntity(entityId).subscribe(
      res => {
        this.workItem = res as WorkItem;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadAssigneeList() {
    this.userService.getUserDictionary().subscribe(
      res => {
        this.assigneeList = res as User[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadWorkItemTypes() {
    this.workItemService.loadWorkItemTypes().subscribe(
      res => {
        this.workItemTypes = res as any[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadWorkItemStatuses() {
    this.workItemService.loadWorkItemStatuses().subscribe(
      res => {
        this.workItemStatuses = res as any[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  removeWorkItem() {
    const projectId = this.workItem.ProjectId;
    this.workItemService.removeItem(this.workItem.Id).subscribe(
      () => {
        this.router.navigate(['/projects/' + projectId]);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
}
