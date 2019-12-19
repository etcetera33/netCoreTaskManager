import { ApiService } from './../api/api.service';
import { User } from './../user/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private rootUrl = this.apiService.rootUrl + 'auth/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  loginWithCredentials(credentials) {
    return this.http.post<any>(this.rootUrl + 'authorize', credentials, this.apiService.headers);
  }

  authorize(token: string) {
    localStorage.setItem('jwt', token);
  }

  logOut() {
    localStorage.removeItem('UserName');
    localStorage.removeItem('jwt');
  }

  register(credentials) {
    return this.http.post(this.rootUrl + 'register', credentials, this.apiService.headers);
  }

  putUserNameInStorage(fullName: string) {
    localStorage.setItem('UserName', fullName);
  }

  getUserName() {
    return localStorage.getItem('UserName');
  }
}
