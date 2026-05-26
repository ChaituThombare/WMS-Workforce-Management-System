import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

import { ProjectService } from '../../../core/services/project.service';
import { ClientService } from '../../../core/services/client.service';

import { Project } from '../../../core/models/project';
import { CreateProject } from '../../../core/models/create-project';
import { Client } from '../../../core/models/client';

@Component({
  selector: 'app-projects',
  standalone: false,
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class Projects implements OnInit {
  private projectService = inject(ProjectService);
  private clientService = inject(ClientService);
  private cdr = inject(ChangeDetectorRef);

  projects: Project[] = [];
  clients: Client[] = [];

  selectedProjectId = 0;
  isEditMode = false;

  projectData: CreateProject = {
    projectName: '',
    clientId: 1,
    startDate: '',
    endDate: '',
    status: 'Active'
  };

  ngOnInit(): void {
    this.loadProjects();
    this.loadClients();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
        this.cdr.detectChanges();
      }
    });
  }

  loadClients(): void {
    this.clientService.getClients().subscribe({
      next: (data) => {
        this.clients = data;
      }
    });
  }

  saveProject(): void {
    if (this.isEditMode) {
      this.projectService.updateProject(
        this.selectedProjectId,
        this.projectData
      ).subscribe({
        next: () => {
          alert('Project updated');
          this.resetForm();
          this.loadProjects();
        }
      });
    } else {
      this.projectService.createProject(this.projectData).subscribe({
        next: () => {
          alert('Project added');
          this.resetForm();
          this.loadProjects();
        }
      });
    }
  }

  editProject(project: Project): void {
    this.selectedProjectId = project.projectId;
    this.isEditMode = true;

    this.projectData = {
      projectName: project.projectName,
      clientId: project.clientId,
      startDate: project.startDate.substring(0, 10),
      endDate: project.endDate.substring(0, 10),
      status: project.status
    };
  }

  cancelEdit(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.selectedProjectId = 0;
    this.isEditMode = false;

    this.projectData = {
      projectName: '',
      clientId: 1,
      startDate: '',
      endDate: '',
      status: 'Active'
    };
  }

  deleteProject(id: number): void {
    if (!confirm('Are you sure you want to delete this project?')) return;

    this.projectService.deleteProject(id).subscribe({
      next: () => {
        this.loadProjects();
      },
      error: () => {
        alert('Delete blocked due to dependencies.');
      }
    });
  }
}