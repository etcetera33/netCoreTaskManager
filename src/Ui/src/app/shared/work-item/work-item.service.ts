import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class WorkItemService {

  private rootUrl = 'https://localhost:44348/api/workItem/';
  private headers =  {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + localStorage.getItem('jwt')
    })
  };
  constructor(private http: HttpClient) { }

  getWorkitemsByProjectId(projectId: number) {
    return this.http.get(this.rootUrl + 'project/' + projectId);
  }

  loadEntity(entityId: number) {
    return this.http.get(this.rootUrl + entityId);
  }

  loadWorkItemTypes() {
    return this.http.get(this.rootUrl + 'types');
  }

  updateEntity(data, id: number) {
    return this.http.put(this.rootUrl + id, data, this.headers);
  }

  loadWorkItemStatuses() {
    return this.http.get(this.rootUrl + 'statuses');
  }

  createEntity(data) {
    return this.http.post(this.rootUrl, data, this.headers);
  }
}
