import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { LoginComponent } from './components/auth/login/login.component';
import { WorkItemDetailComponent } from './components/work-item/work-item-detail/work-item-detail.component';
import { ProjectSettingsComponent } from './components/project/project-settings/project-settings.component';
import { CreateWorkItemComponent } from './components/work-item/create-work-item/create-work-item.component';
import { ProjectDetailComponent } from './components/project/project-detail/project-detail.component';
import { CreateProjectComponent } from './components/project/create-project/create-project.component';
import { ProjectListComponent } from './components/project/project-list/project-list.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './guards/auth-guard';
import { Routes } from '@angular/router';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard]},
    { path: 'projects', component: ProjectListComponent, canActivate: [AuthGuard]},
    { path: 'projects/create', component: CreateProjectComponent, canActivate: [AuthGuard]},
    { path: 'projects/:id', component: ProjectDetailComponent, canActivate: [AuthGuard]},
    { path: 'projects/:id/work-item/create', component: CreateWorkItemComponent, canActivate: [AuthGuard]},
    { path: 'projects/:id/settings', component: ProjectSettingsComponent, canActivate: [AuthGuard]},
    { path: 'projects/:projectId/work-items/:id', component: WorkItemDetailComponent, canActivate: [AuthGuard]},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'auth-callback', component: AuthCallbackComponent},
    { path: '**', component: NotFoundComponent}
];
