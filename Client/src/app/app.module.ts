import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { AddEditEmployeeComponent } from './components/add-edit-employee/add-edit-employee.component';
import { FormsModule } from '@angular/forms';
import { GridModule } from '@syncfusion/ej2-angular-grids';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { TextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { ButtonModule, CheckBoxModule } from '@syncfusion/ej2-angular-buttons';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BaseUrlInterceptor } from './services/base-url.interceptor';
import {
  DropDownListModule,
  DropDownTreeModule,
} from '@syncfusion/ej2-angular-dropdowns';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';

@NgModule({
  declarations: [AppComponent, EmployeeListComponent, AddEditEmployeeComponent],
  imports: [
    BrowserModule,
    FormsModule,
    GridModule,
    HttpClientModule,
    TextBoxModule,
    CheckBoxModule,
    ButtonModule,
    MatDialogModule,
    BrowserAnimationsModule,
    DropDownTreeModule,
    DropDownListModule,
    DatePickerModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BaseUrlInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
