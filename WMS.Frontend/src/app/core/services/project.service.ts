import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Project } from '../models/project';
import { CreateProject } from '../models/create-project';

@Injectable({
    providedIn: 'root'
})

export class ProjectService {
    private http = inject(HttpClient);

    getProjects(): Observable<Project[]> {
        return this.http.get<Project[]>(
           `${environment.apiUrl}/Projects`
        );
    }

    createProject(data: CreateProject): Observable<any> {
        return this.http.post(
            `${environment.apiUrl}/Projects`,
            data
        );
    }

    updateProject(id: number, data: CreateProject): Observable<any> {
        return this.http.put(
            `${environment.apiUrl}/Projects/${id}`,
        data
        );
    }

    deleteProject(id: number): Observable<any> {
        return this.http.delete(
           `${environment.apiUrl}/Projects/${id}`
        );
    }
}