import { ApiService } from './../api/api.service';
import { User } from './user';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private rootUrl = this.apiService.rootUrl + 'user/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getUserDictionary() {
    return this.http.get(this.rootUrl + 'dictionary', this.apiService.headers);
  }

  getUserRoles() {
    return this.http.get(this.rootUrl + 'roles', this.apiService.headers);
  }

  getCurrentUser() {
    return this.http.get<User>(this.rootUrl, this.apiService.headers);
  }
}
