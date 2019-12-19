import { ApiService } from './../api/api.service';
import { Injectable } from '@angular/core';
import {Project} from './project.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private rootUrl = this.apiService.rootUrl + 'project/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getProjectList(page?: number, searchPhrase?: string) {
    let url = this.rootUrl + 'paginate/?';
    if (page != null) {
      url += 'page=' + page + '&';
    }
    if (searchPhrase != null && searchPhrase.length > 0) {
      url += 'search=' + searchPhrase;
    }
    return this.http.get<any>(url, this.apiService.headers);
  }

  createProject(data) {
    return this.http
    .post(this.rootUrl, data, this.apiService.headers);
  }

  getEntity(id: number) {
    return this.http.get(this.rootUrl + id, this.apiService.headers);
  }

  updateProject(data, id: number) {
    return this.http.put(this.rootUrl + id, data, this.apiService.headers);
  }
}
