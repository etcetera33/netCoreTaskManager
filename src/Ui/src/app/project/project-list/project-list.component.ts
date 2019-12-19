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
  pagesCount: number;
  searchPhrase: string;
  currentPage: number;
  constructor(protected projectService: ProjectService) { }

  ngOnInit() {
    this.updateList();
  }

  paginate(page: number) {
    this.currentPage = page;
    this.updateList();
  }

  search() {
    this.updateList();
  }

  updateList() {
    console.log('currentPage: ' + this.currentPage);
    console.log('searchPhrase ' + this.searchPhrase);
    this.projectService.getProjectList(this.currentPage, this.searchPhrase)
    .subscribe(
      res => {
        this.pagesCount = +res.pagesCount;
        this.projectList = res.projectList as Project[];
        console.log('pagesCount: ' + this.pagesCount);
        console.log('projectList: ' + this.projectList);
      },
      err => {
        console.log(err);
      }
    );
  }

}
