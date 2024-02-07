import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategoriesComponent } from './sub-categories.component';
import { RouterModule, Routes } from '@angular/router';
const SUBCATEGORIES_ROUTES: Routes = [
  {
    path: '',
    component: SubCategoriesComponent,
  },
];
@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(SUBCATEGORIES_ROUTES)
  ],
  declarations: [SubCategoriesComponent]
})
export class SubCategoriesModule { }
