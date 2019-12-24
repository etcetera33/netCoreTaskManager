import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
import { NgForm } from '@angular/forms';
import { Commentary } from './../../shared/comment/commentary';
import { CommentService } from './../../shared/comment/comment.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../shared/user/user';
import { Observable } from 'rxjs';
import { AuthService } from '../../shared/auth/auth.service';

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
  constructor(
    private commentService: CommentService, private userService: UserService,
    private popupService: PopupService, private authService: AuthService
    ) { }

  ngOnInit() {
    this.refreshList();
    this.getUser();
  }

  submit(form: NgForm) {
    const formData = JSON.stringify(form.value);
    this.commentService.addComment(formData).subscribe(
      () => {
        this.refreshList();
        form.reset('Body');
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
    this.userId = this.authService.getIdFormToken();
  }

  remove(id: number) {
    this.commentService.removeComment(id).subscribe(
      () => {
        this.refreshList();
      },
      err => {
        console.log(err.status);
        this.popupService.openModal('error', err);
      }
    );
  }

}
