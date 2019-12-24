import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { AuthService } from './../../../services/auth.service';
import { User } from './../../../models/user';
import { NgForm } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: []
})
export class RegisterComponent implements OnInit {

  user: User;
  Password: string;
  RepeartPassword: string;
  roles: any[];
  constructor(private authService: AuthService, private router: Router,
              private userService: UserService, private popupService: PopupService) { }

  ngOnInit() {
    this.userService.getUserRoles().subscribe(
      res => {
        this.roles = res as any[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  register(form: NgForm) {
    const data = JSON.stringify(form.value);
    this.authService.register(data).subscribe(
      res => {
        this.router.navigate(['/login']);
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }
}
