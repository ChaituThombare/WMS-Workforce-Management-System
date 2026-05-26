export interface LoginResponse{
    token: string;
    role: string;
    fullName: string;
    employeeId?: number;
}