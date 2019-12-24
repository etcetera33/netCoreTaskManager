import { PopupService } from './services/popup.service';
import { UserService } from './services/user.service';
import { AuthInterceptor } from './interceptors/auth-interceptor';
import { AuthGuard } from './guards/auth-guard';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { JwtModule } from '@auth0/angular-jwt';

import { RouterModule, Routes, CanActivate } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProfileHeaderComponent } from './components/header/profile-header/profile-header.component';
import { ProjectListComponent } from './components/project/project-list/project-list.component';
import { ProjectDetailComponent } from './components/project/project-detail/project-detail.component';
import { HomeComponent } from './components/home/home.component';
import { CommonModule } from '@angular/common';
import { ProjectService } from './services/project.service';
import { WorkItemService } from './services/work-item.service';
import { WorkItemListComponent } from './components/work-item/work-item-list/work-item-list.component';
import { WorkItemDetailComponent } from './components/work-item/work-item-detail/work-item-detail.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { CreateProjectComponent } from './components/project/create-project/create-project.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ProjectSettingsComponent } from './components/project/project-settings/project-settings.component';
import { CreateWorkItemComponent } from './components/work-item/create-work-item/create-work-item.component';
import { CommentsListComponent } from './components/comments/comments-list/comments-list.component';
import { PopupComponent } from './components/popup/popup.component';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}

const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard]},
  { path: 'projects', component: ProjectListComponent, canActivate: [AuthGuard]},
  { path: 'projects/create', component: CreateProjectComponent, canActivate: [AuthGuard]},
  { path: 'projects/:id', component: ProjectDetailComponent, canActivate: [AuthGuard]},
  { path: 'projects/:id/work-item/create', component: CreateWorkItemComponent, canActivate: [AuthGuard]},
  { path: 'projects/:id/settings', component: ProjectSettingsComponent, canActivate: [AuthGuard]},
  { path: 'projects/:projectId/work-items/:id', component: WorkItemDetailComponent, canActivate: [AuthGuard]},
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
    ProfileHeaderComponent,
    ProjectListComponent,
    ProjectDetailComponent,
    HomeComponent,
    WorkItemListComponent,
    WorkItemDetailComponent,
    LoginComponent,
    RegisterComponent,
    NotFoundComponent,
    CreateProjectComponent,
    ProjectSettingsComponent,
    CreateWorkItemComponent,
    CommentsListComponent,
    PopupComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(
      appRoutes
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
    AuthGuard,
    UserService,
    PopupService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent],
  entryComponents: [PopupComponent]
})
export class AppModule { }
