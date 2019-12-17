import { AuthService } from './../../shared/auth/auth.service';
import { User } from './../../shared/user/user';
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
  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  register(form: NgForm) {
    const data = JSON.stringify(form.value);
    this.authService.register(data).subscribe(
      res => {
        this.router.navigate(['/login']);
      },
      err => {
        console.log(err);
      }
    );
    console.log(data);
  }
}
