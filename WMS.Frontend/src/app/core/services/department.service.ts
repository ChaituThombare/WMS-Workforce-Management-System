import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Department } from '../models/department';
import { CreateDepartment } from '../models/create-department';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
    private http = inject(HttpClient);

    getDepartments(): Observable<Department[]> {
        return this.http.get<Department[]>(
            `${environment.apiUrl}/Departments`
        );
    }

    createDepartment(data: CreateDepartment): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Departments`,
        data
        );
    }

    updateDepartment(id: number, data: CreateDepartment): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Departments/${id}`,
        data
        );
    }

    deleteDepartment(id: number): Observable<any> {
        return this.http.delete(
            `${environment.apiUrl}/Departments/${id}`
        );
    }
}