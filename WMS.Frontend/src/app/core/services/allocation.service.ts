import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Allocation } from '../models/allocation';
import { CreateAllocation } from '../models/create-allocation';

@Injectable({
  providedIn: 'root'
})
export class AllocationService {
    private http = inject(HttpClient);

    getAllocations(): Observable<Allocation[]> {
        return this.http.get<Allocation[]>(
            `${environment.apiUrl}/Allocations`
        );
    }

    getAllocationsByEmployee(id: number): Observable<Allocation[]> {
        return this.http.get<Allocation[]>(
            `${environment.apiUrl}/Allocations/employee/${id}`
        );
    }
    
    createAllocation(data: CreateAllocation): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Allocations`,
            data
        );
    }

    deallocate(id: number): Observable<any> {
        return this.http.put(
           `${environment.apiUrl}/Allocations/deallocate/${id}`,
            {}
        );
    }
}