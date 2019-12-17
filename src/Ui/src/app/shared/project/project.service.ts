import { Injectable } from '@angular/core';
import {Project} from './project.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  rootUrl = 'https://localhost:44348/api/project/';
  headers = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(private http: HttpClient) { }

  getProjectList() {
    return this.http.get(this.rootUrl);
  }

  createProject(data) {
    return this.http
    .post(this.rootUrl, data);
  }

  getEntity(id: number) {
    return this.http.get(this.rootUrl + id);
  }

  updateProject(data, id: number) {
    return this.http.put(this.rootUrl + id, data, this.headers);
  }
}
