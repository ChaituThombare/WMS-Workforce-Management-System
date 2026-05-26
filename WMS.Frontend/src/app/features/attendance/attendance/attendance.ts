import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

import { AttendanceService } from '../../../core/services/attendance.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { AuthService } from '../../../core/services/auth.service';

import { AttendanceRecord } from '../../../core/models/attendance';
import { Employee } from '../../../core/models/employee';
import { CheckInRequest } from '../../../core/models/checkin';
import { CheckOutRequest } from '../../../core/models/checkout';

@Component({
  selector: 'app-attendance',
  standalone: false,
  templateUrl: './attendance.html',
  styleUrl: './attendance.css'
})
export class Attendance implements OnInit {
  private attendanceService = inject(AttendanceService);
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  attendanceRecords: AttendanceRecord[] = [];
  filteredAttendanceRecords: AttendanceRecord[] = [];
  employees: Employee[] = [];

  selectedEmployeeId: number = 0;
  selectedMonth: string = '';
  totalMonthlyHours: number = 0;

  checkInData: CheckInRequest = {
    employeeId: this.authService.getEmployeeId(),
    workMode: 'Office'
  };

  checkOutData: CheckOutRequest = {
    employeeId: this.authService.getEmployeeId()
  };

  ngOnInit(): void {
    if (this.isEmployee()) {
      this.viewEmployeeAttendance();
    } else {
      this.loadEmployees();
      this.loadAttendance();
    }
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = [...data];
         
        if (data.length > 0) {
          this.selectedEmployeeId = data[0].employeeId;
        }

        this.cdr.detectChanges();
      }
    });
  }

  loadAttendance(): void {
    this.attendanceService.getAttendance().subscribe({
      next: (data) => {
        this.attendanceRecords = [...data];
        this.filteredAttendanceRecords = [...data];
        this.calculateTotalHours();
        this.cdr.detectChanges();
      }
    });
  }

  doCheckIn(): void {
    this.attendanceService.checkIn(this.checkInData).subscribe({
      next: () => {
        alert('Check-in successful');
        this.viewEmployeeAttendance();
      },
      error: (err) => {
        alert(err.error.message);
      }
    });
  }

  doCheckOut(): void {
    this.attendanceService.checkOut(this.checkOutData).subscribe({
      next: () => {
        alert('Check-out successful');
        this.viewEmployeeAttendance();
      },
      error: (err) => {
        alert(err.error.message);
      }
    });
  }

  viewEmployeeAttendance(): void {
    const employeeId = this.isEmployee() ? this.authService.getEmployeeId() : this.selectedEmployeeId;

    this.attendanceService
      .getAttendanceByEmployee(employeeId)
      .subscribe({
        next: (data) => {
          this.attendanceRecords = [...data];
          this.applyMonthFilter();
        }
      });
  }

  showAllAttendance(): void {
    this.selectedMonth = '';
    this.loadAttendance();
  }

  applyMonthFilter(): void {
    if (!this.selectedMonth) {
      this.filteredAttendanceRecords = [...this.attendanceRecords];
    } else {
      this.filteredAttendanceRecords = this.attendanceRecords.filter(record => {
        const recordDate = new Date(record.attendanceDate);
        const recordMonth = `${recordDate.getFullYear()}-${String(recordDate.getMonth() + 1).padStart(2, '0')}`;
        return recordMonth === this.selectedMonth;
      });
    }

    this.calculateTotalHours();
    this.cdr.detectChanges();
  }

  calculateTotalHours(): void {
    this.totalMonthlyHours = this.filteredAttendanceRecords.reduce(
      (sum, record) => sum + Number(record.totalHours || 0),
      0
    );
  }
  
  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  isEmployee(): boolean {
    return this.authService.isEmployee();
  }
}