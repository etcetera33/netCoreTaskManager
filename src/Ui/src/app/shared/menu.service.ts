import { Injectable } from '@angular/core';
import { Menu } from './menu.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  menuItems: Array<Menu>;
  
  constructor() { }

  setMenu(menu): void {
    this.menuItems = menu;
  }
}
