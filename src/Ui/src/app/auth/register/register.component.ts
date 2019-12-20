import { UserService } from './../../shared/user/user.service';
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
  roles: any[];
  constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.userService.getUserRoles().subscribe(
      res => {
        this.roles = res as any[];
      },
      err => {
        console.log(err);
      }
    );
  }

  register(form: NgForm) {
    const data = JSON.stringify(form.value);
    console.log(form.value);
    this.authService.register(data).subscribe(
      res => {
        this.router.navigate(['/login']);
      },
      err => {
        console.log(err);
      }
    );
  }
}
