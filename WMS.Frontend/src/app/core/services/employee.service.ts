import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Employee } from '../models/employee';
import { CreateEmployee } from '../models/create-employee';

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
    private http = inject(HttpClient);

    getEmployees(): Observable<Employee[]> {
        return this.http.get<Employee[]>(
            `${environment.apiUrl}/Employees`
        );
    }

    getEmployeeById(id: number): Observable<Employee> {
        return this.http.get<Employee>(
            `${environment.apiUrl}/Employees/${id}`
        );
    }
    
    updateEmployee(id: number, data: CreateEmployee): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Employees/${id}`,
            data
        );
    }

    createEmployee(data: CreateEmployee): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Employees`,
            data
        );
    }

    deleteEmployee(id: number): Observable<any> {
        return this.http.delete(
            `${environment.apiUrl}/Employees/${id}`
        );
    }
}

