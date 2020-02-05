import { UserService } from './../../services/user.service';
import { AuthService } from './../../services/auth.service';
import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styles: []
})
export class AuthCallbackComponent implements OnInit {

  constructor(private router: Router, private oauthService: OAuthService, private route: ActivatedRoute,
              private authService: AuthService, private userService: UserService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.spinner.show();

    setTimeout(() => {
      this.oauthService.tokenValidationHandler = new JwksValidationHandler();
      const jwt = this.oauthService.getAccessToken();

      localStorage.setItem('jwt', jwt);
      this.userService.loadUser();
      this.authService.putRoleToLocalStorage(jwt);

      this.spinner.hide();
      this.router.navigate(['/']);
    }, 3000);
  }

}
