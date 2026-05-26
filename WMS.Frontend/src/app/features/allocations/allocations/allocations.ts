import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { AllocationService } from '../../../core/services/allocation.service';
import { EmployeeService } from '../../../core/services/employee.service';
import { ProjectService } from '../../../core/services/project.service';

import { Allocation } from '../../../core/models/allocation';
import { CreateAllocation } from '../../../core/models/create-allocation';
import { Employee } from '../../../core/models/employee';
import { Project } from '../../../core/models/project';

@Component({
  selector: 'app-allocations',
  standalone: false,
  templateUrl: './allocations.html',
  styleUrl: './allocations.css',
})
export class Allocations implements OnInit {
  private allocationService = inject(AllocationService);
  private employeeService = inject(EmployeeService);
  private projectService = inject(ProjectService);
  private cdr = inject(ChangeDetectorRef);

  allocations: Allocation[] = [];
  employees: Employee[] = [];
  projects: Project[] = [];

  allocationData: CreateAllocation = {
    empId: 1,
    projectId: 1,
    createdBy: 'admin'
  };

  ngOnInit(): void {
    this.loadAllocations();
    this.loadEmployees();
    this.loadProjects();
  }

  loadAllocations(): void {
    this.allocationService.getAllocations().subscribe({
      next: (data) => {
        this.allocations = data;
        this.cdr.detectChanges();
      }
    });
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      }
    });
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
      }
    });
  }

  createAllocation(): void {
    this.allocationService.createAllocation(this.allocationData).subscribe({
      next: () => {
        alert('Allocation created');
        this.loadAllocations();
      }
    });
  }

  deallocate(id: number): void {
    if (!confirm('Deallocate employee?')) return;

    this.allocationService.deallocate(id).subscribe({
      next: () => {
        this.loadAllocations();
      }
    });
  }
}
