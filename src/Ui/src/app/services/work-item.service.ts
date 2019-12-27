import { ApiService } from './api.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class WorkItemService {
  private projectId: number;
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getWorkitemsByProjectId(assigneeId: number, page?: number, searchPhrase?: string) {
    const objectUrl: any = {};
    if (page != null) {
      objectUrl.page = page;
    }
    if (searchPhrase != null && searchPhrase.length > 0) {
      objectUrl.search = searchPhrase;
    }
    if (assigneeId != null) {
      objectUrl.assigneeId = assigneeId;
    }
    const params = new HttpParams({
      fromObject: objectUrl
    });
    return this.http.get<any>(this.getRootUrl(), {params});
  }

  loadEntity(entityId: number) {
    return this.http.get(this.getRootUrl() + entityId);
  }

  loadWorkItemTypes() {
    return this.http.get(this.getRootUrl() + 'types');
  }

  updateEntity(data, id: number) {
    return this.http.put(this.getRootUrl() + id, data);
  }

  loadWorkItemStatuses() {
    return this.http.get(this.getRootUrl() + 'statuses');
  }

  createEntity(data) {
    return this.http.post(this.getRootUrl(), data);
  }

  private getRootUrl() {
    return this.apiService.rootUrl + 'projects/' + this.projectId + '/workitems/';
  }

  setProjectId(projectId: number) {
    this.projectId = projectId;
  }
}
