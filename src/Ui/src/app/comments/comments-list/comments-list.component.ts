import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
import { NgForm, NgModel } from '@angular/forms';
import { Commentary } from './../../shared/comment/commentary';
import { CommentService } from './../../shared/comment/comment.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../shared/user/user';

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
  user: User;
  constructor(private commentService: CommentService, private userService: UserService, private popupService: PopupService) { }

  ngOnInit() {
    this.refreshList();
    this.getUser();
  }

  submit(form: NgForm) {
    const formData = JSON.stringify(form.value);
    this.commentService.addComment(formData).subscribe(
      res => {
        this.refreshList();
        form.controls['Body'].reset();
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  refreshList() {
    this.commentService.loadCommentsByProjectId(this.workItemId).subscribe(
      res => {
        this.commentList = res as Commentary[];
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

  getUser() {
    this.user = this.userService.getCurrentUser();
  }

  remove(id: number) {
    this.commentService.removeComment(id).subscribe(
      res => {
        this.refreshList();
      },
      err => {
        this.popupService.openModal('error', err);
      }
    );
  }

}
