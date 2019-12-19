import { Router } from '@angular/router';
import { UserService } from './../../shared/user/user.service';
import { AuthService } from './../../shared/auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-header',
  templateUrl: './profile-header.component.html',
  styles: []
})
export class ProfileHeaderComponent implements OnInit {
  fullName: string;
  constructor(private authService: AuthService, private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.fullName = this.authService.getUserName();
  }

  logout() {
    this.authService.logOut();
    this.router.navigate(['/login']);
  }

}
