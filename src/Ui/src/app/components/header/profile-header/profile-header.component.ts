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
  fullName: string;
  constructor(private authService: AuthService, private userService: UserService, private router: Router) { }

  ngOnInit() {
    const user = this.userService.getCurrentUser();
    if (user !== undefined) {
      this.fullName = user.FullName;
    }
  }

  logout() {
    this.authService.logOut();
    this.router.navigate(['/login']);
  }

}
