import { UserService } from './../../shared/user/user.service';
import { RouterModule, Routes, Router } from '@angular/router';
import { AuthService } from './../../shared/auth/auth.service';
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
  constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    const credentials = JSON.stringify(form.value);
    this.authService.loginWithCredentials(credentials).toPromise().then(
      response => {
      const token = response.token;
      console.log('AUTHORIZING');
      this.authService.authorize(token);
      this.putUserNameInStorage();
      this.router.navigate(['/']);
    },
    err => {
      console.log(err);
    });
  }

  putUserNameInStorage() {
    this.userService.getCurrentUser().toPromise()
    .then(
      res => {
        this.authService.putUserNameInStorage(res.FullName);
      },
      err => {
        console.log(err);
      }
    );
  }

}
