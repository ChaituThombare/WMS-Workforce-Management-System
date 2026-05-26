import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AttendanceRecord } from '../models/attendance';
import { CheckInRequest } from '../models/checkin';
import { CheckOutRequest } from '../models/checkout';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {
    private http = inject(HttpClient);

    getAttendance(): Observable<AttendanceRecord[]> {
        return this.http.get<AttendanceRecord[]>(
        `${environment.apiUrl}/Attendance`
        );
    }

    getAttendanceByEmployee(employeeId: number): Observable<AttendanceRecord[]> {
        return this.http.get<AttendanceRecord[]>(
            `${environment.apiUrl}/Attendance/employee/${employeeId}`
        );
    }
    
    checkIn(data: CheckInRequest): Observable<any> {
        return this.http.post(
        `${environment.apiUrl}/Attendance/check-in`,
        data
        );
    }

    checkOut(data: CheckOutRequest): Observable<any> {
        return this.http.post(
        `${environment.apiUrl}/Attendance/checkout`,
        data
        );
    }
}