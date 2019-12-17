import { CommentService } from './../../shared/comment/comment.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styles: []
})
export class CommentsListComponent implements OnInit {
  commentList: Comment[];
  constructor(private commentService: CommentService) { }

  ngOnInit() {
  }

}
