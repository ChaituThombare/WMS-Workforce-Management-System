export interface CreateEmployee {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    gender: string;
    dob: string;
    doj: string;
    departmentId: number;
    roleId: number;
    status: string;
    password?: string;
}