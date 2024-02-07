import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoriesComponent } from './categories.component';
import { RouterModule, Routes } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
const CATEGORIES_ROUTES: Routes = [
  {
    path: '',
    component: CategoriesComponent,
  },
];
@NgModule({
  imports: [
    CommonModule,
    MatSidenavModule,
    RouterModule.forChild(CATEGORIES_ROUTES)
  ],
  declarations: [CategoriesComponent]
})
export class CategoriesModule { }
