import { Component, inject, ChangeDetectorRef, OnInit, ViewChild, NgZone } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardSummary } from '../../../core/models/dashboard-summary';
import { AuthService } from '../../../core/services/auth.service';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private dashboardService = inject(DashboardService);
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);
  authService = inject(AuthService);

  @ViewChild('barChart') barChart?: BaseChartDirective;
  @ViewChild('pieChart') pieChart?: BaseChartDirective;

  username = localStorage.getItem('fullName') || 'User';

  summary: DashboardSummary = {
    totalEmployees: 0,
    totalProjects: 0,
    activeAllocations: 0,
    pendingLeaves: 0,
    todayAttendance: 0,

    activeEmployees: 0,
    activeProjects: 0,

    myProjects: 0,
    myPendingLeaves: 0,
    myAttendanceCount: 0,
    myAverageHours: 0,

    employeesOnLeave: 0
  };

  adminBarChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['Employees', 'Projects', 'Allocations', 'Attendance'],
    datasets: [
      {
        data: [],
        label: 'Workforce Analytics',
        backgroundColor: [
          '#2563eb',
          '#10b981',
          '#f59e0b',
          '#6366f1'
        ],
        borderRadius: 8,
      },
    ],
  };

  adminBarChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
      }
    }
  };

  adminPieChartData: ChartConfiguration<'doughnut'>['data'] = {
    labels: ['Active Allocations', 'Pending Leaves', 'Employees On Leave'],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#2563eb',
          '#ef4444',
          '#10b981'
        ],
        hoverOffset: 10,
      }
    ]
  };

  adminPieChartType: ChartType = 'doughnut';

  employeePieChartData: ChartConfiguration<'doughnut'>['data'] = {
    labels: [
      'Pending Leaves',
      'Projects',
      'Attendance',
    ],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#2563eb',
          '#10b981',
          '#f59e0b'
        ],
        hoverOffset: 10,
      }
    ]
  };

  employeePieChartType: ChartType = 'doughnut';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.dashboardService.getSummary().subscribe({
      next: (data) => {
        this.summary = data;

        this.adminBarChartData.datasets[0].data = [
          data.totalEmployees,
          data.totalProjects,
          data.activeAllocations,
          data.todayAttendance
        ];

        this.adminPieChartData.datasets[0].data = [
          data.activeAllocations,
          data.pendingLeaves,
          data.employeesOnLeave
        ];

        this.employeePieChartData.datasets[0].data = [
          data.pendingLeaves,
          data.totalProjects,
          data.todayAttendance
        ];

        this.cdr.detectChanges();

        this.ngZone.runOutsideAngular(() => {
          setTimeout(() => {
            this.barChart?.chart?.update();
            this.pieChart?.chart?.update();
          }, 300);
        });
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