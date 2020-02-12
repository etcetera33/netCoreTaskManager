import { NgxSpinnerService } from 'ngx-spinner';
import { PopupService } from './../../../../services/popup.service';
import { UserService } from './../../../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../../../models/user';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}
const ELEMENT_DATA: PeriodicElement[] = [
  {position: 1, name: 'Hydrogen', weight: 1.0079, symbol: 'H'},
  {position: 2, name: 'Helium', weight: 4.0026, symbol: 'He'},
  {position: 3, name: 'Lithium', weight: 6.941, symbol: 'Li'},
  {position: 4, name: 'Beryllium', weight: 9.0122, symbol: 'Be'},
  {position: 5, name: 'Boron', weight: 10.811, symbol: 'B'},
  {position: 6, name: 'Carbon', weight: 12.0107, symbol: 'C'},
  {position: 7, name: 'Nitrogen', weight: 14.0067, symbol: 'N'},
  {position: 8, name: 'Oxygen', weight: 15.9994, symbol: 'O'},
  {position: 9, name: 'Fluorine', weight: 18.9984, symbol: 'F'},
  {position: 10, name: 'Neon', weight: 20.1797, symbol: 'Ne'},
];

@Component({
  selector: 'app-admin-users-list',
  templateUrl: './admin-users-list.component.html',
  styles: []
})
export class AdminUsersListComponent implements OnInit {
  pagesCount: number;
  searchPhrase: string;
  currentPage: number;
  displayedColumns: string[] = ['Id', 'FullName', 'Email', 'Position', 'Role', 'Detail'];
  dataSource = ELEMENT_DATA;
  userList: User[] = [
    {Id: 1, FullName: 'John Doe', Email: 'john.doe@gmail.com', Position: 'Position', Role: 'role', RoleId: 2},
    {Id: 1, FullName: 'John Doe', Email: 'john.doe@gmail.com', Position: 'Position', Role: 'role', RoleId: 2},
  ];
  constructor(private userService: UserService, private popupService: PopupService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.updateList();
  }

  search() {
    this.updateList();
  }

  paginate(page: number) {
    this.currentPage = page;
    this.updateList();
  }

  updateList() {
    this.spinner.show();
    this.userService.getAll(this.currentPage, this.searchPhrase)
    .subscribe(
      res => {
        this.pagesCount = +res.PagesCount;
        this.userList = res.EntityList as User[];
        this.spinner.hide();
      },
      err => {
        this.spinner.hide();
        this.popupService.openModal('error', err);
      }
    );
  }
}
