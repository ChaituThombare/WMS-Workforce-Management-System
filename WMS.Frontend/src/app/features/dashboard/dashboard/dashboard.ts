import { Component, inject, ChangeDetectorRef, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardSummary } from '../../../core/models/dashboard-summary';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private dashboardService = inject(DashboardService);
  private cdr = inject(ChangeDetectorRef);
  authService = inject(AuthService);

  username = localStorage.getItem('fullName') || 'User';

  summary: DashboardSummary = {
    totalEmployees: 0,
    totalDepartments: 0,
    totalClients: 0,
    totalProjects: 0,
    activeAllocations: 0,
    pendingLeaves: 0,
    todayAttendance: 0
  };

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.dashboardService.getSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Dashboard API Error:', err);
      }
    });
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  isEmployee(): boolean {
    return this.authService.isEmployee();
  }
}