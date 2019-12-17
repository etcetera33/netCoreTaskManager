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
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    const credentials = JSON.stringify(form.value);
    this.authService.loginWithCredentials(credentials).subscribe(response => {
      const token = (response as any).token;
      this.authService.authorize(token);
      this.invalidLogin = false;
      this.router.navigate(['/']);
    },
    err => {
      this.invalidLogin = true;
    });
  }

}
