import { PopupService } from './../../../services/popup.service';
import { UserService } from './../../../services/user.service';
import { Router } from '@angular/router';
import { AuthService } from './../../../services/auth.service';
import { NgForm } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {

  Login: string;
  Password: string;
  constructor(
    private authService: AuthService, private router: Router,
    private userService: UserService, private popupService: PopupService
    ) { }

  ngOnInit() {
    console.log('inside login method1');
    this.authService.login();
  }

  login(form: NgForm) {
    const credentials = JSON.stringify(form.value);
    this.authService.loginWithCredentials(credentials).toPromise().then(
      response => {
      const token = response.token;
      this.authService.authorize(token);
      this.userService.loadUser();
      this.authService.putRoleToLocalStorage(token);
      this.router.navigate(['/']);
    },
    err => {
      this.popupService.openModal('error', err);
    });
  }


}
