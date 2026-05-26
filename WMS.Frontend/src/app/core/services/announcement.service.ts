import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Announcement } from '../models/announcement';

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {
  private http = inject(HttpClient);

  getAll(): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(
      `${environment.apiUrl}/Announcements`
    );
  }

  getActive(): Observable<Announcement[]> {
    return this.http.get<Announcement[]>(
      `${environment.apiUrl}/Announcements/active`
    );
  }

  create(data: any): Observable<any> {
    return this.http.post(
      `${environment.apiUrl}/Announcements`,
      data
    );
  }

  toggleStatus(id: number): Observable<any> {
    return this.http.put(
      `${environment.apiUrl}/Announcements/toggle/${id}`,
      {}
    );
  }
}