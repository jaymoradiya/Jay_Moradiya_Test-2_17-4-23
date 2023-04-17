import { Sort } from './sort.enum';

export interface FilterParams {
  sort: Sort | null;
  orderBy: string;
  department: string[] | null;
  gender: string[] | null;
}
