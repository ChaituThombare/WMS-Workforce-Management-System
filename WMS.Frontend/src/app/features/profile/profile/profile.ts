import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { EmployeeService } from '../../../core/services/employee.service';
import { AuthService } from '../../../core/services/auth.service';
import { Employee } from '../../../core/models/employee';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit {
  private employeeService = inject(EmployeeService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  employee?: Employee;

  ngOnInit(): void {
    const employeeId = this.authService.getEmployeeId();

    if (employeeId) {
      this.employeeService.getEmployeeById(employeeId).subscribe({
        next: (data) => {
          this.employee = data;
          this.cdr.detectChanges();
        }
      });
    }
  }
}