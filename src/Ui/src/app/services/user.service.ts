import { PopupService } from './popup.service';
import { WorkItem } from './../models/work-item.model';
import { ApiService } from './api.service';
import { User } from './../models/user';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private rootUrl = this.apiService.rootUrl + 'users/';
  private currentUser: User;
  constructor(private http: HttpClient, private apiService: ApiService, private popupService: PopupService) {}

  getUserDictionary() {
    return this.http.get(this.rootUrl + 'dictionary');
  }

  getUserRoles() {
    return this.http.get(this.rootUrl + 'roles');
  }

  getCurrentUser() {
    if (this.currentUser === undefined) {
      const t = this.http.get<User>(this.rootUrl).toPromise();
      this.loadUser();
    }
    return this.currentUser;
  }

  loadUser() {
    this.http.get<User>(this.rootUrl).subscribe(
      res => {
        this.currentUser = res;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  getCurrentUserTasks() {
    return this.http.get<WorkItem[]>(this.rootUrl + 'current/work-items');
  }

  getCurrentRole() {
    return localStorage.getItem('role');
  }
}
