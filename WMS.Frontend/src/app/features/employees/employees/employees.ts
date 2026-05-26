import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

import { EmployeeService } from '../../../core/services/employee.service';
import { Employee } from '../../../core/models/employee';
import { CreateEmployee } from '../../../core/models/create-employee';
import { Department } from '../../../core/models/department';
import { DepartmentService } from '../../../core/services/department.service';

@Component({
  selector: 'app-employees',
  standalone: false,
  templateUrl: './employees.html',
  styleUrl: './employees.css',
})
export class Employees implements OnInit {
  private employeeService = inject(EmployeeService);
  private cdr = inject(ChangeDetectorRef);
  private departmentService = inject(DepartmentService);

  employees: Employee[] = [];
  filteredEmployees: Employee[] = [];
  departments: Department[] = [];

  selectedEmployeeId = 0;
  isEditMode = false;

  searchText = '';
  selectedDepartment = '';
  selectedRole = '';

  employeeData: CreateEmployee = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    gender: '',
    dob: '',
    doj: '',
    departmentId: 1,
    roleId: 2,
    password: '',
    status: 'Active'
  };

  ngOnInit(): void {
    this.loadEmployees();
    this.loadDepartments();
  }

  loadEmployees(): void {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.filteredEmployees = data;
        this.cdr.detectChanges();
      }
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.cdr.detectChanges();
      }
    });
  }

  filterEmployees(): void {
    this.filteredEmployees = this.employees.filter(emp => {
      const fullName = `${emp.firstName} ${emp.lastName}`.toLowerCase();

      const matchesSearch =
        this.searchText === '' ||
        emp.employeeId.toString().includes(this.searchText) ||
        fullName.includes(this.searchText.toLowerCase());

      const matchesDepartment =
        this.selectedDepartment === '' ||
        emp.departmentName === this.selectedDepartment;

      const matchesRole =
        this.selectedRole === '' ||
        emp.roleName === this.selectedRole;

      return matchesSearch && matchesDepartment && matchesRole;
    });
  }

  resetFilters(): void {
    this.searchText = '';
    this.selectedDepartment = '';
    this.selectedRole = '';
    this.filteredEmployees = this.employees;
  }

  saveEmployee(): void {
    if(this.isEditMode){
      this.employeeService.updateEmployee(
        this.selectedEmployeeId,
        this.employeeData
      ).subscribe({
        next: () => {
          alert('Employee updated successfully!');
          this.resetForm();
          this.loadEmployees();
        }
      });
    }else{
      const generatedPassword = `${this.employeeData.lastName}@123`;
      this.employeeData.password = generatedPassword;

      this.employeeService.createEmployee(this.employeeData).subscribe({
        next: () => {
          alert(
            `User created successfully\nUsername: ${this.employeeData.firstName.toLowerCase()}\nPassword: ${generatedPassword}`
          );

          this.resetForm();
          this.loadEmployees();
        }
      });
    }
  }

  editEmployee(emp: Employee): void {
    this.selectedEmployeeId = emp.employeeId;
    this.isEditMode = true;

    this.employeeData = {
      firstName: emp.firstName,
      lastName: emp.lastName,
      email: emp.email,
      phoneNumber: emp.phoneNumber,
      gender: emp.gender,
      dob: emp.dob.substring(0, 10),
      doj: emp.doj.substring(0, 10),
      departmentId: 1,
      roleId: 2,
      password: '',
      status: emp.status
    };
  }

  cancelEdit(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.selectedEmployeeId = 0;
    this.isEditMode = false;
    
    this.employeeData = {
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
      gender: '',
      dob: '',
      doj: '',
      departmentId: 1,
      roleId: 2,
      password: '',
      status: 'Active'
    };
  }

  deleteEmployee(id: number): void {
    if (!confirm('Are you sure you want to delete this employee?')) return;

    this.employeeService.deleteEmployee(id).subscribe({
      next: () => {
        this.loadEmployees();
      },
      error: () => {
        alert('Delete failed. Employee may have dependencies.');
      }
    });
  }

  getDepartments(): string[] {
    return [...new Set(this.employees.map(e => e.departmentName))];
  }

  getRoles(): string[] {
    return [...new Set(this.employees.map(e => e.roleName))];
  }
}
