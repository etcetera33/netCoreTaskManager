import { PopupService } from './../popup/popup.service';
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
  private currentUser: User;
  constructor(private http: HttpClient, private apiService: ApiService, private popupService: PopupService) {
    this.loadUser();
  }

  getUserDictionary() {
    return this.http.get(this.rootUrl + 'dictionary');
  }

  getUserRoles() {
    return this.http.get(this.rootUrl + 'roles');
  }

  getCurrentUser() {
    if (this.currentUser === undefined) {
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
    return this.currentUser.Role;
  }
}
