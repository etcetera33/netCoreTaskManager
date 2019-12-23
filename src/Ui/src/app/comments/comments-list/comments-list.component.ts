import { UserService } from './../../shared/user/user.service';
import { User } from './../../shared/user/user';
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
  userId: number;
  constructor(private commentService: CommentService, private userService: UserService) { }

  ngOnInit() {
    this.refreshList();
    this.getUserId();
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

  getUserId() {
    this.userId = this.userService.getCurrentUser().Id;
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
