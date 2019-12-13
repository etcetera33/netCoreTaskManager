import { MenuComponent } from './../menu/menu.component';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styles: []
})
export class ProjectListComponent implements OnInit {
  @Input() menu: MenuComponent;
  constructor() { }

  ngOnInit() {
  }

}
