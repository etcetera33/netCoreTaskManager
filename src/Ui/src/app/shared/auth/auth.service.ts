import { User } from './../user/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  rootUrl = 'https://localhost:44348/api/auth/';
  headers = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + localStorage.getItem('jwt')
    })
  };
  constructor(private client: HttpClient) { }

  loginWithCredentials(credentials) {
    return this.client.post(this.rootUrl + 'authorize', credentials, this.headers);
  }

  authorize(token: string) {
    localStorage.setItem('jwt', token);
  }

  logOut() {
    localStorage.removeItem('jwt');
  }

  register(credentials) {
    return this.client.post(this.rootUrl + 'register', credentials, this.headers);
  }
}
