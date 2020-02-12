import { NgForm } from '@angular/forms';
import { PopupService } from './popup.service';
import { WorkItem } from './../models/work-item.model';
import { ApiService } from './api.service';
import { User } from './../models/user';
import { HttpClient, HttpParams } from '@angular/common/http';
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
      this.loadUser();
    }
    return this.currentUser;
  }

  loadUser() {
    this.http.get<User>(this.rootUrl + 'current').subscribe(
      res => {
        this.currentUser = res;
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  getAll(page?: number, searchPhrase?: string) {
    const objectUrl: any = {};
    if (page != null) {
      objectUrl.page = page;
    }
    if (searchPhrase != null && searchPhrase.length > 0) {
      objectUrl.search = searchPhrase;
    }
    const params = new HttpParams({
      fromObject: objectUrl
    });
    return this.http.get<any>(this.rootUrl + '?', {params});
  }

  getCurrentUserTasks() {
    return this.http.get<WorkItem[]>(this.rootUrl + 'current/work-items');
  }

  getCurrentRole() {
    return localStorage.getItem('role');
  }

  getById(id: number) {
    return this.http.get(this.rootUrl + id);
  }

  moderate(data, id: number) {
    return this.http.put(this.rootUrl + id, data);
  }
}
