import { UserService } from './../../shared/user/user.service';
import { WorkItem } from './../../shared/work-item/work-item.model';
import { Component, OnInit, Input } from '@angular/core';
import { WorkItemService } from '../../shared/work-item/work-item.service';

@Component({
  selector: 'app-workitem-list',
  templateUrl: './work-item-list.component.html',
  styles: []
})
export class WorkItemListComponent implements OnInit {

  @Input()
  projectId: number;

  assigneeId: number;
  workItems: WorkItem[];
  constructor(protected workItemService: WorkItemService, protected userService: UserService) { }

  ngOnInit() {
    if (this.projectId != null) {
      this.workItemService.getWorkitemsByProjectId(this.projectId).subscribe(
        res => {
          this.workItems = res as WorkItem[];
        },
        err => {
          console.log(err);
        }
      );
    }
  }

}
