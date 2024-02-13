import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategoriesComponent } from './sub-categories.component';
import { RouterModule, Routes } from '@angular/router';
import { EditSubCategoryComponent } from './edit-sub-category/edit-sub-category.component';
import { UnsavedChangesGuard } from 'src/app/core/guards/unsaved-changes.guard';
import { NoSelectionComponent } from './no-selection/no-selection.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { SharedModule } from 'src/app/core/shared/shared.module';
const SUBCATEGORIES_ROUTES: Routes = [
  {
    path: '',
    component: SubCategoriesComponent,
    children: [
      { path: '', component: NoSelectionComponent },
      { path: 'category/:categoryId/create', component: EditSubCategoryComponent },
      { path: ':id/edit', component: EditSubCategoryComponent, canDeactivate: [UnsavedChangesGuard] }
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
    RouterModule.forChild(SUBCATEGORIES_ROUTES)
  ],
  declarations: [SubCategoriesComponent, EditSubCategoryComponent]
})
export class SubCategoriesModule { }
