import { WorkItem } from './../work-item/work-item.model';
import { ApiService } from './../api/api.service';
import { User } from './user';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private rootUrl = this.apiService.rootUrl + 'users/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getUserDictionary() {
    return this.http.get(this.rootUrl + 'dictionary');
  }

  getUserRoles() {
    return this.http.get(this.rootUrl + 'roles');
  }

  getCurrentUser() {
    return this.http.get<User>(this.rootUrl);
  }

  getCurrentUserTasks() {
    return this.http.get<WorkItem[]>(this.rootUrl + 'current/work-items');
  }
}
