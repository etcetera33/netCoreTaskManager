import { Injectable } from '@angular/core';
import {WorkItem} from './work-item.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class WorkItemService {

  workItem: WorkItem;
  workItemList: WorkItem[];
  rootUrl = 'https://localhost:44348/api/workItem/';
  constructor(private http: HttpClient) { }

  getWorkitemsByProjectId(projectId: number) {
    return this.http.get(this.rootUrl + 'project/' + projectId);
  }

  loadEntity(entityId: number) {
    return this.http.get(this.rootUrl + entityId);
  }
}
