import { UserService } from './../shared/user/user.service';
import { WorkItem } from './../shared/work-item/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {

  workItems: WorkItem[];
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrentUserTasks().subscribe(
      res => {
        this.workItems = res as WorkItem[];
      },
      err => {
        console.log(err);
      }
    );
  }

}
