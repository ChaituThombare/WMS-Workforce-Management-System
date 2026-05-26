import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

import { LeaveService } from '../../../core/services/leave.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { AuthService } from '../../../core/services/auth.service';

import { LeaveRequest } from '../../../core/models/leave';
import { ApplyLeaveRequest } from '../../../core/models/apply-leave';
import { LeaveActionRequest } from '../../../core/models/leave-action';
import { Employee } from '../../../core/models/employee';

@Component({
  selector: 'app-leave',
  standalone: false,
  templateUrl: './leave.html',
  styleUrl: './leave.css',
})
export class Leave implements OnInit {
  private leaveService = inject(LeaveService);
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  leaves: LeaveRequest[] = [];
  employees: Employee[] = [];

  leaveData: ApplyLeaveRequest = {
    employeeId: 1,
    startDate: '',
    endDate: '',
    leaveType: '',
    reason: ''
  };

  actionData: LeaveActionRequest = {
    managerComments: '',
    approvedBy: 'admin'
  };

  ngOnInit(): void {
    if (this.isEmployee()) {
      this.leaveData.employeeId = this.authService.getEmployeeId();
      this.viewEmployeeLeaves();
    } else {
      this.loadEmployees();
      this.loadLeaves();
    }
  }

  loadLeaves(): void{
    this.leaveService.getLeaves().subscribe({
      next: (data) => {
        this.leaves = [...data];
        this.cdr.detectChanges();
      }
    });
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = [...data];
        this.cdr.detectChanges();
      }
    });
  }

  applyLeave(): void{
    this.leaveService.applyLeave(this.leaveData).subscribe({
      next: () => {
        alert('Leave applied successfully!');
        this.viewEmployeeLeaves();
      }
    });
  }

  approveLeave(id: number): void {
    const comments = prompt('Enter manager comments for approval:');

    if(comments === null) return;

    const actionData = {
      managerComments: comments,
      approvedBy: localStorage.getItem('username') || 'admin'
    };

    this.leaveService.approveLeave(id, actionData).subscribe({
      next: () => {
        alert('Leave approved!');
        this.loadLeaves();
      }
    });
  }

  rejectLeave(id: number): void {
    const comments = prompt('Enter rejection reason:');

    if(comments === null) return;

    const actionData = {
      managerComments: comments,
      approvedBy: localStorage.getItem('username') || 'admin'
    };

    this.leaveService.rejectLeave(id, actionData).subscribe({
      next: () => {
        alert('Leave rejected!');
        this.loadLeaves();
      }
    });
  }

  cancelLeave(id: number): void{
    this.leaveService.cancelLeave(id).subscribe({
      next: () => {
        alert('Leave cancelled!');
        
        if(this.isEmployee()){
          this.viewEmployeeLeaves();
        }else{
          this.loadLeaves();
        }
      }
    });
  }

  viewEmployeeLeaves(): void{
    this.leaveService.getLeavesByEmployee(this.leaveData.employeeId).subscribe({
      next: (data) => {
        this.leaves = [...data];
        this.cdr.detectChanges();
      }
    });
  }

  showAllLeaves(): void {
    this.loadLeaves();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  isEmployee(): boolean {
    return this.authService.isEmployee();
  }
}
