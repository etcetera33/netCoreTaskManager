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

  invalidLogin: boolean;
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
      this.authService.authorize(token);
      this.invalidLogin = false;
      console.log('LOGINING');
    },
    err => {
      this.invalidLogin = true;
    })
    .then(() => {
      console.log('PUTING NAME IN THE STORAGE');
      this.putUserNameInStorage();
    })
    .then(() => {
      this.router.navigate(['/']);
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
