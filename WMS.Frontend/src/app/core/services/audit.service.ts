import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuditLog } from '../models/audit-log';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  private http = inject(HttpClient);

  getLogs(): Observable<AuditLog[]> {
    return this.http.get<AuditLog[]>(
      `${environment.apiUrl}/AuditLogs`
    );
  }
}