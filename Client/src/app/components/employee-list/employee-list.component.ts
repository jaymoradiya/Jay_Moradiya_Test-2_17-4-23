import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Employee } from 'src/app/models/employee.model';
import { EmployeeService } from 'src/app/services/employee.service';
import { AddEditEmployeeComponent } from '../add-edit-employee/add-edit-employee.component';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ApiResponse } from 'src/app/models/api-response.model';
import { Observable } from 'rxjs';
import { Department } from 'src/app/models/department.enum';
import { DropDownTreeComponent } from '@syncfusion/ej2-angular-dropdowns';
import { FilterParams } from 'src/app/models/filter-params.model';
import { Sort } from 'src/app/models/sort.enum';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
})
export class EmployeeListComponent implements OnInit {
  @ViewChild('grid')
  grid: GridComponent | null = null;

  isImmutable = false;

  employee: Employee[] = [];
  query = '';
  sortBy = '';
  orderBy = 'desc';

  filterData: { [key: string]: Object }[] = [
    {
      name: 'Gender',
      hasChild: true,
      expanded: false,
      child: [{ name: 'Male' }, { name: 'Female' }],
    },
    {
      name: 'Department',
      hasChild: true,
      expanded: false,
      child: [
        ...Object.values(Department).map((v) => {
          return {
            name: v,
          };
        }),
      ],
    },
  ];

  filterFields: Object = {
    dataSource: this.filterData,
    value: 'name',
    text: 'name',
    hasChildren: 'hasChild',
  };
  treeSettings: Object = { autoCheck: true };

  filterOptions: FilterParams = {
    gender: null,
    department: null,
    orderBy: '',
    sort: null,
  };

  constructor(
    private employeeService: EmployeeService,
    private dialog: MatDialog
  ) {
    employeeService.employees.subscribe((employees) => {
      this.grid?.setProperties({
        dataSource: employees,
      });
    });
  }

  ngOnInit(): void {
    this.getAllEmployees();
  }

  getAllEmployees() {
    this.employeeService.getAllEmployees(this.filterOptions).subscribe();
  }

  onEmployeeDelete(employee: Employee) {
    this.employeeService.deleteEmployee(employee.id).subscribe();
  }

  onAdd() {
    this.dialog.open(AddEditEmployeeComponent, {
      width: '400px',
    });
  }

  onEdit(program: Employee) {
    this.isImmutable = true;
    this.dialog.open(AddEditEmployeeComponent, {
      width: '400px',
      data: program,
    });
  }

  onSearch(query: string) {
    if (query === '') {
      return this.getAllEmployees();
    }
    this.employeeService.searchEmployee(query).subscribe();
  }
  onFilter(dropdowns: DropDownTreeComponent) {
    this.filterOptions.department = null;
    this.filterOptions.gender = null;
    dropdowns.value?.forEach((v) => {
      if (v === 'Male' || v === 'Female') {
        if (this.filterOptions.gender) this.filterOptions.gender.push(v);
        else this.filterOptions.gender = [v];
      } else {
        if (this.filterOptions.department)
          this.filterOptions.department.push(v);
        else this.filterOptions.department = [v];
      }
    });
    if (this.sortBy != '')
      this.filterOptions.sort =
        this.sortBy === 'department' ? Sort.Department : Sort.Gender;
    if (this.orderBy != '') this.filterOptions.orderBy = this.orderBy;

    console.log(this.orderBy);
    console.log(this.sortBy);
    console.log(this.filterOptions);
    this.employeeService.getAllEmployees(this.filterOptions).subscribe();
  }
}
