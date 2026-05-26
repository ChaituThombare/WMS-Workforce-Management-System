import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Client } from '../models/client';
import { CreateClient } from '../models/create-client';

@Injectable({
    providedIn: 'root'
})
export class ClientService {
    private http = inject(HttpClient);

    getClients(): Observable<Client[]> {
        return this.http.get<Client[]>(
          `${environment.apiUrl}/Clients`
        );
    }

    createClient(data: CreateClient): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Clients`,
        data
        );
    }

    updateClient(id: number, data: CreateClient): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Clients/${id}`,
        data
        );
    }

    deleteClient(id: number): Observable<any> {
        return this.http.delete(
            `${environment.apiUrl}/Clients/${id}`
        );
    }
}