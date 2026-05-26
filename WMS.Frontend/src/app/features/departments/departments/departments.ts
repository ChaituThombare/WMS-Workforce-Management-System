import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/department';
import { CreateDepartment } from '../../../core/models/create-department';

@Component({
  selector: 'app-departments',
  standalone: false,
  templateUrl: './departments.html',
  styleUrl: './departments.css',
})
export class Departments {
  private departmentService = inject(DepartmentService);
  private cdr = inject(ChangeDetectorRef);

  departments: Department[] = []
  selectedDepartmentId = 0;
  isEditMode = false;
  
  departmentData: CreateDepartment = {
    departmentName: '',
    description: ''
  };

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.detectChanges();
      }
    });
  }

  saveDepartment(): void {
    if(this.isEditMode){
      this.departmentService.updateDepartment(
        this.selectedDepartmentId,
        this.departmentData
      ).subscribe({
        next: () => {
          alert('Department updated successfully!');
          this.resetForm();
          this.loadDepartments();
        }
      });
    }else{
      this.departmentService.createDepartment(this.departmentData).subscribe({
        next: () => {
          alert('Department created successfully!');
          this.resetForm();
          this.loadDepartments();
        }
      });
    }
  }

  editDepartment(dept: Department): void {
    this.selectedDepartmentId = dept.departmentId;
    this.isEditMode = true;
    
    this.departmentData = {
      departmentName: dept.departmentName,
      description: dept.description
    };
  }

  cancelEdit(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.selectedDepartmentId = 0;
    this.isEditMode = false;
    
    this.departmentData = {
      departmentName: '',
      description: ''
    };
  }

  deleteDepartment(id: number): void {
    if(!confirm('Are you sure you want to delete this department?')) return;
      
    this.departmentService.deleteDepartment(id).subscribe({
      next: () => {
        this.loadDepartments();
      },
      error: () => {
        alert('Delete blocked due to dependencies.');
      }
    });
  }
}
