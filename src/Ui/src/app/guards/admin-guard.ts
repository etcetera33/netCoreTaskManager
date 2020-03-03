import { AuthService } from './../services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(private jwtHelper: JwtHelperService, private router: Router, private authService: AuthService) {
    }
    canActivate() {
      const token = localStorage.getItem('jwt');
      if (token && !this.jwtHelper.isTokenExpired(token) && this.jwtHelper.decodeToken(token).Role === 'Admin' ) {
        return true;
      }

      this.router.navigate(['']);
      return false;
    }
}
