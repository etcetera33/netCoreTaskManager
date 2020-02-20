import { Router } from '@angular/router';
import { UserService } from './../../../services/user.service';
import { AuthService } from './../../../services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-header',
  templateUrl: './profile-header.component.html',
  styles: []
})
export class ProfileHeaderComponent implements OnInit {
  role: string;
  constructor(private authService: AuthService, private router: Router, private userService: UserService) { }

  ngOnInit() {
    this.role = this.userService.getCurrentRole();
  }

  logout() {
    this.authService.logOut();
    this.router.navigate(['/login']);
  }

}
