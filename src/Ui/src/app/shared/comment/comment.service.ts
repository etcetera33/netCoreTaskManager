import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private rootUrl = 'https://localhost:44348/api/comment/';
  constructor(private http: HttpClient) { }

  loadCommentsByProjectId(workItemId: number) {
    return this.http.get(this.rootUrl + 'work-item/' + workItemId);
  }

  addComment(data) {

  }

  removeComment(id: number) {

  }
}
