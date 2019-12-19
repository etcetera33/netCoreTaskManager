import { ApiService } from './../api/api.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class WorkItemService {
  private rootUrl = this.apiService.rootUrl + 'workItem/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getWorkitemsByProjectId(projectId: number) {
    return this.http.get(this.rootUrl + 'project/' + projectId + '/current-user', this.apiService.headers);
  }

  loadEntity(entityId: number) {
    return this.http.get(this.rootUrl + entityId, this.apiService.headers);
  }

  loadWorkItemTypes() {
    return this.http.get(this.rootUrl + 'types', this.apiService.headers);
  }

  updateEntity(data, id: number) {
    return this.http.put(this.rootUrl + id, data, this.apiService.headers);
  }

  loadWorkItemStatuses() {
    return this.http.get(this.rootUrl + 'statuses', this.apiService.headers);
  }

  createEntity(data) {
    return this.http.post(this.rootUrl, data, this.apiService.headers);
  }
}
