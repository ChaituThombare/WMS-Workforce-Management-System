export interface DashboardSummary {
    totalEmployees: number;
    activeEmployees: number;

    totalProjects: number;
    activeProjects: number;

    activeAllocations: number;
    pendingLeaves: number;
    todayAttendance: number;

    myProjects: number;
    myPendingLeaves: number;
    myAttendanceCount: number;
    myAverageHours: number;

    employeesOnLeave: number;
}