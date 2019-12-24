import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { WorkItem } from './../../../models/work-item.model';
import { Component, OnInit, Input } from '@angular/core';
import { WorkItemService } from './../../../services/work-item.service';
import { User } from './../../../models/user';

@Component({
  selector: 'app-workitem-list',
  templateUrl: './work-item-list.component.html',
  styles: []
})
export class WorkItemListComponent implements OnInit {

  @Input()
  projectId: number;

  searchPhrase: string;
  currentPage: number;
  assigneeId: number;
  pagesCount: number;
  workItems: WorkItem[];
  users: User[];
  constructor(protected workItemService: WorkItemService, protected userService: UserService, private popupService: PopupService) { }

  ngOnInit() {
    this.workItemService.setProjectId(this.projectId);
    this.updateList();
    this.loadUserDictionary();
  }

  paginate(page: number) {
    this.currentPage = page;
    this.updateList();
  }

  search() {
    this.updateList();
  }

  updateList() {
    this.workItemService.getWorkitemsByProjectId(this.assigneeId, this.currentPage, this.searchPhrase)
    .subscribe(
      res => {
        this.pagesCount = +res.pagesCount;
        this.workItems = res.wokrItemList as WorkItem[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadUserDictionary() {
    this.userService.getUserDictionary().subscribe(
      res => {
        this.users = res as User[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
