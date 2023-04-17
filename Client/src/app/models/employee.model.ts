import { Department } from './department.enum';

export interface Employee {
  id: number;
  name: string;
  email: string;
  gender: string;
  dateOfBirth: string;
  department: Department;
}
