export interface LeaveRequest {
  leaveRequestId: number;
  employeeId: number;
  employeeName: string;
  startDate: string;
  endDate: string;
  leaveType: string;
  reason: string;
  status: string;
  managerComments: string | null;
  approvedBy: string | null;
  approvedOn: string | null;
  appliedOn: string;
}