import { AuthGuard } from './shared/auth/auth-guard';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { JwtModule } from '@auth0/angular-jwt';

import { RouterModule, Routes, CanActivate } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { SearchComponent } from './header/search/search.component';
import { ProfileHeaderComponent } from './header/profile-header/profile-header.component';
import { ProjectListComponent } from './project/project-list/project-list.component';
import { ProjectDetailComponent } from './project/project-detail/project-detail.component';
import { HomeComponent } from './home/home.component';
import { CommonModule } from '@angular/common';
import { ProjectService } from './shared/project/project.service';
import { WorkItemService } from './shared/work-item/work-item.service';
import { WorkItemListComponent } from './work-item/work-item-list/work-item-list.component';
import { WorkItemDetailComponent } from './work-item/work-item-detail/work-item-detail.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { CreateProjectComponent } from './project/create-project/create-project.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ProjectSettingsComponent } from './project/project-settings/project-settings.component';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}

const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard]},
  { path: 'projects', component: ProjectListComponent, canActivate: [AuthGuard]},
  { path: 'projects/create', component: CreateProjectComponent, canActivate: [AuthGuard]},
  { path: 'projects/:id', component: ProjectDetailComponent, canActivate: [AuthGuard]},
  { path: 'projects/:id/settings', component: ProjectSettingsComponent, canActivate: [AuthGuard]},
  { path: 'work-items/:id', component: WorkItemDetailComponent, canActivate: [AuthGuard]},
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', component: NotFoundComponent}
];

const jwtConfig = {
  config: {
    tokenGetter,
    whitelistedDomains: ['localhost:5000'],
    blacklistedRoutes: []
  }
};

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    SearchComponent,
    ProfileHeaderComponent,
    ProjectListComponent,
    ProjectDetailComponent,
    MenuComponent,
    HomeComponent,
    WorkItemListComponent,
    WorkItemDetailComponent,
    LoginComponent,
    RegisterComponent,
    NotFoundComponent,
    CreateProjectComponent,
    ProjectSettingsComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    ),
    JwtModule.forRoot(jwtConfig),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MatDialogModule,
    NoopAnimationsModule
  ],
  providers: [
    ProjectService,
    WorkItemService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
