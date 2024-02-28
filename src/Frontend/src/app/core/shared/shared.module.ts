import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableControlBarComponent } from 'src/app/core/shared/components/table-control-bar/table-control-bar.component';
import { FilterListComponent } from 'src/app/core/shared/components/filter-list/filter-list.component';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { FilterMenuComponent } from 'src/app/core/shared/components/filter-menu/filter-menu.component';
import { FilterComponent } from 'src/app/core/shared/components/filter/filter.component';
// import { FilterBooleanFormComponent } from 'src/app/core/shared/components/filter-boolean-form/filter-boolean-form.component';
// import { FilterDateFormComponent } from 'src/app/core/shared/components/filter-date-form/filter-date-form.component';
// import { FilterNumberFormComponent } from 'src/app/core/shared/components/filter-number-form/filter-number-form.component';
// import { FilterSelectFormComponent } from 'src/app/core/shared/components/filter-select-form/filter-select-form.component';
// import { FilterStringFormComponent } from 'src/app/core/shared/components/filter-string-form/filter-string-form.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { FilterBooleanFormComponent } from 'src/app/core/shared/components/filter-boolean-form/filter-boolean-form.component';
import { FilterDateFormComponent } from 'src/app/core/shared/components/filter-date-form/filter-date-form.component';
import { FilterNumberFormComponent } from 'src/app/core/shared/components/filter-number-form/filter-number-form.component';
import { FilterSelectFormComponent } from 'src/app/core/shared/components/filter-select-form/filter-select-form.component';
import { FilterStringFormComponent } from 'src/app/core/shared/components/filter-string-form/filter-string-form.component';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { FilterService } from 'src/app/core/shared/services/filter.service';
import { AnswerTypePipe } from 'src/app/core/pipes/answer-type.pipe';
import { NoSelectionComponent } from 'src/app/core/shared/components/no-selection/no-selection.component';
import { NavbarComponent } from 'src/app/core/navbar/navbar.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    TableControlBarComponent,
    FilterListComponent,
    FilterMenuComponent,
    FilterComponent,
    FilterBooleanFormComponent,
    FilterDateFormComponent,
    FilterNumberFormComponent,
    FilterSelectFormComponent,
    FilterStringFormComponent,
    NoSelectionComponent,
    AnswerTypePipe,
    NavbarComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatMenuModule,
    MatCheckboxModule,
    MatChipsModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatButtonModule,
    MatSelectModule,
    MatInputModule,
    ReactiveFormsModule,
    MatDialogModule,
    RouterModule
  ],
  exports: [
    TableControlBarComponent,
    FilterListComponent,
    FilterMenuComponent,
    NoSelectionComponent,
    AnswerTypePipe,
    NavbarComponent
  ],
  providers: [
    FilterService
  ]
})
export class SharedModule { }
