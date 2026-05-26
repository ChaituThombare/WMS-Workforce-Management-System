export interface AuditLog {
  auditId: number;
  entityName: string;
  recordId: number;
  action: string;
  createdBy: string;
  createdOn: string;
}