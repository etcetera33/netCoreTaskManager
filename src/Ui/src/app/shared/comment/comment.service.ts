import { ApiService } from './../api/api.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private rootUrl = this.apiService.rootUrl + 'comments/';
  constructor(private http: HttpClient, private apiService: ApiService) { }

  loadCommentsByProjectId(workItemId: number) {
    return this.http.get(this.rootUrl + 'work-item/' + workItemId);
  }

  addComment(data) {
    return this.http.post(this.rootUrl, data);
  }

  removeComment(id: number) {
    return this.http.delete(this.rootUrl + id);
  }
}
