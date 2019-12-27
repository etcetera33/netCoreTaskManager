import { ApiService } from './api.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private rootUrl = this.apiService.rootUrl + 'auth/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  loginWithCredentials(credentials) {
    return this.http.post<any>(this.rootUrl + 'authorize', credentials);
  }

  authorize(token: string) {
    localStorage.setItem('jwt', token);
  }

  setRole(role: string) {
    localStorage.setItem('role', role);
  }

  logOut() {
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
    this.setRole(this.parseToken(token).role);
  }

  public getIdFormToken() {
    return this.parseToken(localStorage.getItem('jwt')).unique_name;
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


}
