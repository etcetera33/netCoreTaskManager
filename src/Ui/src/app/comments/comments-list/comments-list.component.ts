import { PopupService } from './../../shared/popup/popup.service';
import { UserService } from './../../shared/user/user.service';
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
  constructor(private commentService: CommentService, private userService: UserService, private popupService: PopupService) { }

  ngOnInit() {
    this.refreshList();
    this.getUserId();
  }

  submit(form: NgForm) {
    const formData = JSON.stringify(form.value);
    this.commentService.addComment(formData).subscribe(
      res => {
        this.refreshList();
        if (this.comment) {
          console.log('Inside');
          this.comment.Body = '';
        }
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

  getUserId() {
    this.userId = this.userService.getCurrentUser().Id;
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
