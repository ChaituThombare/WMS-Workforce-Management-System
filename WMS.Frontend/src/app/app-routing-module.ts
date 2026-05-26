import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainLayout } from './shared/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { Login } from './features/auth/login/login';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { Employees } from './features/employees/employees/employees';
import { Departments } from './features/departments/departments/departments';
import { Clients } from './features/clients/clients/clients';
import { Projects } from './features/projects/projects/projects';
import { Allocations } from './features/allocations/allocations/allocations';
import { Attendance } from './features/attendance/attendance/attendance';
import { Leave } from './features/leave/leave/leave';
import { Profile } from './features/profile/profile/profile';
import { MyProjects } from './features/my-projects/my-projects/my-projects';
import { Announcements } from './features/announcements/announcements/announcements';
import { AuditLogs } from './features/audit-logs/audit-logs/audit-logs';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'},

  { path: 'login', component: Login },
  
  { path: '', component: MainLayout, canActivate: [authGuard],
    children: [
        { path: 'dashboard', component: Dashboard},
        { path: 'attendance', component: Attendance},
        { path: 'leave', component: Leave },
        { path: 'employees', component: Employees, canActivate: [roleGuard] },
        { path: 'departments', component: Departments, canActivate: [roleGuard] },
        { path: 'clients', component: Clients, canActivate: [roleGuard] },
        { path: 'projects', component: Projects, canActivate: [roleGuard] },
        { path: 'allocations', component: Allocations, canActivate: [roleGuard] },
        { path: 'profile', component: Profile },
        { path: 'my-projects', component: MyProjects },
        { path: 'announcements', component: Announcements},
        { path: 'audit-logs', component: AuditLogs}
      ]
  },
  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
