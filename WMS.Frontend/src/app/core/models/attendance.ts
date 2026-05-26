export interface AttendanceRecord {
  attendanceId: number;
  employeeId: number;
  employeeName: string;
  attendanceDate: string;
  checkInTime: string;
  checkOutTime: string | null;
  totalHours: number;
  workMode: string;
}