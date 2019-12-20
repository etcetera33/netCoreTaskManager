import { UserService } from './../../shared/user/user.service';
import { WorkItem } from './../../shared/work-item/work-item.model';
import { Component, OnInit, Input } from '@angular/core';
import { WorkItemService } from '../../shared/work-item/work-item.service';
import { User } from '../../shared/user/user';

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
  constructor(protected workItemService: WorkItemService, protected userService: UserService) { }

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
        console.log(err);
      }
    );
  }

  loadUserDictionary() {
    this.userService.getUserDictionary().subscribe(
      res => {
        this.users = res as User[];
      },
      err => {
        console.log(err);
      }
    );
  }

}
