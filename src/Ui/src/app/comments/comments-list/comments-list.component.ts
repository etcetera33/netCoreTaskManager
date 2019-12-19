import { NgForm } from '@angular/forms';
import { Commentary } from './../../shared/comment/commentary';
import { CommentService } from './../../shared/comment/comment.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styles: []
})
export class CommentsListComponent implements OnInit {
  @Input()
  workItemId: number;

  commentList: Commentary[];
  comment: Commentary;
  constructor(private commentService: CommentService) { }

  ngOnInit() {
    this.refreshList();
  }

  submit(form: NgForm) {
    const formData = JSON.stringify(form.value);
    console.log(formData);
    this.commentService.addComment(formData).subscribe(
      res => {
        this.refreshList();
        this.comment = null;
      },
      err => {
        console.log(err);
      }
    );
  }

  refreshList() {
    this.commentService.loadCommentsByProjectId(this.workItemId).subscribe(
      res => {
        this.commentList = res as Commentary[];
      },
      err => {
        console.log(err);
      }
    );
  }

  remove(id: number) {
    this.commentService.removeComment(id).subscribe(
      res => {
        this.refreshList();
      },
      err => {
        console.log(err);
      }
    );
  }

}
