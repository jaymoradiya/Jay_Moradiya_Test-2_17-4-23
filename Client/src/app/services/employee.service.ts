import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, map } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { environment } from 'src/environments/environment.development';
import { Employee } from '../models/employee.model';
import { FilterParams } from '../models/filter-params.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  Api = environment.Api;
  employees = new BehaviorSubject<Employee[]>([]);

  constructor(private http: HttpClient) {}

  getAllEmployees(
    filterParams: FilterParams
  ): Observable<ApiResponse<Employee[]>> {
    let params = new HttpParams();
    params = params.append('sort', filterParams.sort?.toString() ?? '');
    params = params.append('orderBy', filterParams.orderBy);
    if (filterParams.department)
      params = params.append(
        'departments',
        filterParams.department?.join(',') ?? ''
      );
    if (filterParams.gender)
      params = params.append('genders', filterParams.gender?.join(',') ?? '');

    return this.http
      .get<ApiResponse<Employee[]>>(this.Api.allEmployees, {
        params: params,
      })
      .pipe(
        map((res) => {
          if (res.status) {
            this.employees.next(res.data);
          }
          return res;
        })
      );
  }

  addEmployee(employee: Employee): Observable<ApiResponse<Employee>> {
    return this.http
      .post<ApiResponse<Employee>>(this.Api.addEmployee, employee)
      .pipe(
        map((res) => {
          if (res.status) {
            this.employees.next([...this.employees.getValue(), res.data]);
          }
          return res;
        })
      );
  }

  editEmployee(employee: Employee): Observable<ApiResponse> {
    return this.http
      .put<ApiResponse>(this.Api.updateEmployee + employee.id, employee)
      .pipe(
        map((res) => {
          if (res.status) {
            const updatedEmployees = this.employees
              .getValue()
              .reduce((acc: Employee[], crr) => {
                if (crr.id === employee.id) {
                  crr = employee;
                }
                acc.push(crr);
                return acc;
              }, []);
            this.employees.next(updatedEmployees);
          }
          return res;
        })
      );
  }

  deleteEmployee(id: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(this.Api.deleteEmployee + id).pipe(
      map((res) => {
        if (res.status) {
          const updateEmployees = this.employees
            .getValue()
            .reduce((acc: Employee[], crr) => {
              if (crr.id !== id) {
                acc.push(crr);
              }
              return acc;
            }, []);
          this.employees.next(updateEmployees);
        }
        return res;
      })
    );
  }

  searchEmployee(query: string): Observable<ApiResponse<Employee[]>> {
    return this.http
      .get<ApiResponse<Employee[]>>(`${this.Api.searchEmployee}`, {
        params: { query },
      })
      .pipe(
        map((res) => {
          if (res.status) {
            this.employees.next(res.data);
          }
          return res;
        })
      );
  }
}
