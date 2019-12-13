import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { SearchComponent } from './search/search.component';
import { ProfileHeaderComponent } from './profile-header/profile-header.component';
import { ProjectListComponent } from './project-list/project-list.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { HomeComponent } from './home/home.component';
import { CommonModule } from '@angular/common';

const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'projects', component: ProjectListComponent},
  { path: 'projects/:id', component: ProjectDetailComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    SearchComponent,
    ProfileHeaderComponent,
    ProjectListComponent,
    ProjectDetailComponent,
    MenuComponent,
    HomeComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    ),
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
