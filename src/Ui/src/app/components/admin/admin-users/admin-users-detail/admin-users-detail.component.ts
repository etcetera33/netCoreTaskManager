import { NgForm } from '@angular/forms';
import { PopupService } from './../../../../services/popup.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from '../../../../models/user';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from './../../../../services/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-users-detail',
  templateUrl: './admin-users-detail.component.html',
  styles: []
})
export class AdminUsersDetailComponent implements OnInit {
  userId: number;
  user: User;
  roles: any[];
  constructor(private userService: UserService, private activatedRoute: ActivatedRoute,
              private spinner: NgxSpinnerService, private popupService: PopupService,
              private router: Router) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.userId = +params.get('id');
      this.loadUserInfo();
    });

    this.spinner.show();
    this.userService.getUserRoles().subscribe(
      res => {
        this.roles = res as any[];
        this.spinner.hide();
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  loadUserInfo() {
    this.spinner.show();
    this.userService.getById(this.userId).subscribe(
      res => {
        this.user = res as User;
        this.spinner.hide();
      },
    err => {
      this.spinner.hide();
      this.popupService.openModal('error', err);
    });
  }

  moderate(form: NgForm) {
    this.spinner.show();
    const data = JSON.stringify(form.value);
    this.userService.moderate(data, this.user.Id).subscribe(
      res => {
        this.spinner.hide();
        this.router.navigate(['/admin/users']);
      },
      err => {
        this.spinner.hide();
        this.popupService.openModal('error', err);
      }
    );
  }
}
