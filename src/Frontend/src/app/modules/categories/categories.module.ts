import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories.component';
import { RouterModule, Routes } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { EditCategoryComponent } from 'src/app/modules/categories/edit-category/edit-category.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'src/app/core/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { UnsavedChangesGuard } from 'src/app/core/guards/unsaved-changes.guard';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NoSelectionComponent } from './no-selection/no-selection.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

const CATEGORIES_ROUTES: Routes = [
  {
    path: '',
    component: CategoriesComponent,
    children: [
      { path: '', component: NoSelectionComponent },
      { path: 'create', component: EditCategoryComponent },
      { path: ':id/edit', component: EditCategoryComponent, canDeactivate: [UnsavedChangesGuard] }
    ]
  },
];
@NgModule({
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    RouterModule.forChild(CATEGORIES_ROUTES)
  ],
  declarations: [CategoriesComponent, EditCategoryComponent, NoSelectionComponent]
})
export class CategoriesModule { }
