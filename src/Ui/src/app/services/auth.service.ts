import { ApiService } from './api.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  // Observable navItem source
  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this.authNavStatusSource.asObservable();
  private rootUrl = this.apiService.rootUrl + 'auth/';
  private userManager = new UserManager(this.getUserManagerSettngs());
  private user: User | null;
  constructor(private http: HttpClient, private apiService: ApiService, private oauthService: OAuthService) { }

  loginWithCredentials(credentials) {
    return this.http.post<any>(this.rootUrl + 'authorize', credentials);
  }

  login() {
    this.oauthService.initLoginFlow();
  }

  public get name() {
    const claims = this.oauthService.getIdentityClaims();
    if (!claims) {
      return null;
    }

    return claims;
}

  authorize(token: string) {
    localStorage.setItem('jwt', token);
  }

  setRole(role: string) {
    localStorage.setItem('role', role);
  }

  logOut() {
    this.oauthService.logOut();
    localStorage.removeItem('jwt');
  }

  register(credentials) {
    return this.http.post(this.rootUrl + 'register', credentials);
  }

  public parseToken(token: string = '') {
    if (token === null || token === '') { return { upn: '' }; }
    const parts = token.split('.');
    if (parts.length !== 3) {

        throw new Error('JWT must have 3 parts');
    }
    const decoded = this.urlBase64Decode(parts[1]);
    if (!decoded) {
        throw new Error('Cannot decode the token');
    }
    return JSON.parse(decoded);
  }

  public putRoleToLocalStorage(token: string = '') {
    this.setRole(this.parseToken(token).Role);
  }

  public getIdFormToken() {
    return this.parseToken(localStorage.getItem('jwt')).id;
  }

  private urlBase64Decode(str: string) {
    let output = str.replace(/-/g, '+').replace(/_/g, '/');
    switch (output.length % 4) {
        case 0:
            break;
        case 2:
            output += '==';
            break;
        case 3:
            output += '=';
            break;
        default:
            // tslint:disable-next-line:no-string-throw
            throw 'Illegal base64url string!';
    }
    return decodeURIComponent((<any>window).escape(window.atob(output)));
  }

  refreshToken() {
    this.oauthService.refreshToken();
  }

  getUserManagerSettngs(): UserManagerSettings {
    return {
      authority: 'https://localhost:44306',
      client_id: 'angular_spa',
      redirect_uri: 'http://localhost:4200/auth-callback',
      post_logout_redirect_uri: 'http://localhost:4200/',
      response_type: 'id_token token',
      scope: 'openid profile email api.read',
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
    };
  }

  async completeAuthentication() {
    this.user = await this.userManager.signinRedirectCallback();
    this.authNavStatusSource.next(this.isAuthenticated());
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }
}
