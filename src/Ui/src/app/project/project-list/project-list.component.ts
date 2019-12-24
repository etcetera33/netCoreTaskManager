import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
import { Project } from './../../shared/project/project.model';
import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../shared/project/project.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styles: []
})
export class ProjectListComponent implements OnInit {
  projectList: Project[];
  pagesCount: number;
  searchPhrase: string;
  currentPage: number;
  role: string;
  constructor(protected projectService: ProjectService, protected userService: UserService, private popupService: PopupService) { }

  ngOnInit() {
    this.updateList();
    this.role = this.userService.getCurrentRole();
  }

  paginate(page: number) {
    this.currentPage = page;
    this.updateList();
  }

  search() {
    this.updateList();
  }

  updateList() {
    this.projectService.getProjectList(this.currentPage, this.searchPhrase)
    .subscribe(
      res => {
        this.pagesCount = +res.pagesCount;
        this.projectList = res.projectList as Project[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
