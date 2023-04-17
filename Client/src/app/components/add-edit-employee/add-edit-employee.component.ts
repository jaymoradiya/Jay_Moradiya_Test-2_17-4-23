import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ApiResponse } from 'src/app/models/api-response.model';
import { Department } from 'src/app/models/department.enum';
import { Employee } from 'src/app/models/employee.model';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-add-edit-employee',
  templateUrl: './add-edit-employee.component.html',
})
export class AddEditEmployeeComponent {
  public targetElement: HTMLElement | null = null;
  @Input()
  isEditMode = false;
  @Input()
  employee: Employee = {
    id: 0,
    name: '',
    email: '',
    gender: '',
    dateOfBirth: '',
    department: Department.development,
  };

  selectedDate: Date | null = null;

  departments = Object.values(Department);

  constructor(
    private employeeService: EmployeeService,
    public dialogRef: MatDialogRef<AddEditEmployeeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Employee | undefined
  ) {
    if (data) {
      this.employee = data;
      this.selectedDate = new Date(this.employee.dateOfBirth);
      this.isEditMode = true;
    }
  }

  onSave() {
    // console.log(this.employee);
    // return;
    this.employee.dateOfBirth = this.formatDate(this.selectedDate!);
    let employeeObservable: Observable<ApiResponse<any>>;
    if (this.isEditMode) {
      employeeObservable = this.employeeService.editEmployee(this.employee!);
    } else {
      employeeObservable = this.employeeService.addEmployee(this.employee!);
    }
    employeeObservable.subscribe();
    this.dialogRef.close();
  }

  onCancel() {
    this.dialogRef.close();
  }

  formatDate(date: Date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
  }
}
