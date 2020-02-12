import { AuthService } from './../../services/auth.service';
import { PopupService } from './../../services/popup.service';
import { UserService } from './../../services/user.service';
import { WorkItem } from './../../models/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {

  workItems: WorkItem[];
  constructor(private userService: UserService, private popupService: PopupService, private authService: AuthService) { }

  ngOnInit() {
    this.userService.getCurrentUserTasks().subscribe(
      res => {
        this.workItems = res as WorkItem[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
