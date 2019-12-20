import { ApiService } from './../api/api.service';
import { Injectable } from '@angular/core';
import {Project} from './project.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private rootUrl = this.apiService.rootUrl + 'projects/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  getProjectList(page?: number, searchPhrase?: string) {
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

  createProject(data) {
    return this.http.post(this.rootUrl, data);
  }

  getEntity(id: number) {
    return this.http.get(this.rootUrl + id);
  }

  updateProject(data, id: number) {
    return this.http.put(this.rootUrl + id, data);
  }
}
