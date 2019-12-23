import { ApiService } from './../api/api.service';
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

  logOut() {
    localStorage.removeItem('jwt');
  }

  register(credentials) {
    return this.http.post(this.rootUrl + 'register', credentials);
  }
}
