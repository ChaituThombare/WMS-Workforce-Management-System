import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { LeaveRequest } from '../models/leave';
import { ApplyLeaveRequest } from '../models/apply-leave';
import { LeaveActionRequest } from '../models/leave-action';

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
    private http = inject(HttpClient);

    getLeaves(): Observable<LeaveRequest[]> {
        return this.http.get<LeaveRequest[]>(
            `${environment.apiUrl}/Leave`
        );
    }

    getLeavesByEmployee(employeeId: number): Observable<LeaveRequest[]> {
        return this.http.get<LeaveRequest[]>(
            `${environment.apiUrl}/Leave/employee/${employeeId}`
        );
    }

    applyLeave(data: ApplyLeaveRequest): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Leave/apply`,
            data
        );
    }

    approveLeave(id: number, data: LeaveActionRequest): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Leave/approve/${id}`,
            data
        );
    }

    rejectLeave(id: number, data: LeaveActionRequest): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Leave/reject/${id}`,
            data
        );
    }

    cancelLeave(id: number): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Leave/cancel/${id}`,
            {}
        );
    }
}