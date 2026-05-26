import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    
    login(data: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(
            `${environment.apiUrl}/Auth/login`,
            data
        ).pipe(
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    saveAuthData(response: LoginResponse): void {
        localStorage.setItem('token', response.token);
        localStorage.setItem('fullName', response.fullName);
        localStorage.setItem('role', response.role);
        if(response.employeeId){
            localStorage.setItem('employeeId', response.employeeId.toString());
        }
    }

    getToken(): string | null {
        return localStorage.getItem('token');
    }

    getRole(): string {
        return localStorage.getItem('role') || '';
    }

    getEmployeeId(): number{
        return Number(localStorage.getItem('employeeId'));
    }
    
    isAdmin(): boolean {
        const role = this.getRole();
        return role === 'Admin';
    }

    isEmployee(): boolean {
        return this.getRole() === 'Employee';
    }

    logout(): void {
        localStorage.clear();
    }

    isLoggedIn(): boolean{
        return !!this.getToken();
    }
}