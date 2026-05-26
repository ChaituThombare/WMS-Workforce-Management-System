export interface ApplyLeaveRequest {
  employeeId: number;
  startDate: string;
  endDate: string;
  leaveType: string;
  reason: string;
}