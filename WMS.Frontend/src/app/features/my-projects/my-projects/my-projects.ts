import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { AllocationService } from '../../../core/services/allocation.service';
import { AuthService } from '../../../core/services/auth.service';
import { Allocation } from '../../../core/models/allocation';

@Component({
  selector: 'app-my-projects',
  standalone: false,
  templateUrl: './my-projects.html',
  styleUrl: './my-projects.css'
})
export class MyProjects implements OnInit {
  private allocationService = inject(AllocationService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  allocations: Allocation[] = [];

  ngOnInit(): void {
    const employeeId = this.authService.getEmployeeId();
    console.log('Employee ID:', employeeId);

    if (employeeId) {
      this.allocationService.getAllocationsByEmployee(employeeId).subscribe({
        next: (data) => {
          this.allocations = data;
          this.cdr.detectChanges();
        }
      });
    }
  }
}