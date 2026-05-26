import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuditService } from '../../../core/services/audit.service';
import { AuditLog } from '../../../core/models/audit-log';

@Component({
  selector: 'app-audit-logs',
  standalone: false,
  templateUrl: './audit-logs.html',
  styleUrl: './audit-logs.css',
})
export class AuditLogs implements OnInit {
  private auditService = inject(AuditService);
  private cdr = inject(ChangeDetectorRef);

  logs: AuditLog[] = [];

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.auditService.getLogs().subscribe({
      next: (data) => {
        this.logs = [...data];
        this.cdr.detectChanges();
      }
    });
  }
}