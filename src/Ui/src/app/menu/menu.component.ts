import { Component, OnInit, Input, NgModule } from '@angular/core';
import { Menu } from '../shared/menu.model';
import { CommonModule } from '@angular/common';


@NgModule({
  declarations:
  [
    CommonModule
  ]
})
@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styles: []
})
export class MenuComponent implements OnInit {
  menuItems: Array<Menu>;

  constructor() {}

  ngOnInit() {}

  setMenuItems(menu): void {
    this.menuItems = menu;
  }
}
