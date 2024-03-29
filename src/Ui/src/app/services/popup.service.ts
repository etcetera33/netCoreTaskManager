import { PopupComponent } from './../components/popup/popup.component';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class PopupService {

  constructor(public dialog: MatDialog) { }
  openModal(title: string, response: any) {
    const dialogConfig = new MatDialogConfig();

    dialogConfig.autoFocus = true;
    dialogConfig.data = {
        title,
        message: response.status
    };
    dialogConfig.minWidth = 400;

    const dialogRef = this.dialog.open(PopupComponent, dialogConfig);
  }
}
