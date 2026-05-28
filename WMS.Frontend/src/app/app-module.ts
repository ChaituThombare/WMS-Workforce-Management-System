import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Navbar } from './shared/layout/navbar/navbar';
import { Sidebar } from './shared/layout/sidebar/sidebar';
import { MainLayout } from './shared/layout/main-layout/main-layout';
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
import { BaseChartDirective, provideCharts, withDefaultRegisterables } from 'ng2-charts';

@NgModule({
  declarations: [
    App,
    Navbar,
    Sidebar,
    MainLayout,
    Login,
    Dashboard,
    Employees,
    Departments,
    Clients,
    Projects,
    Allocations,
    Attendance,
    Leave,
    Profile,
    MyProjects,
    Announcements,
    AuditLogs,
  ],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, FormsModule, BaseChartDirective],

  providers: [
    provideBrowserGlobalErrorListeners(),
    provideCharts(withDefaultRegisterables()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [App],
})
export class AppModule {}
