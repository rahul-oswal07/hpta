import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionsComponent } from './questions.component';
import { RouterModule, Routes } from '@angular/router';
const QUESTIONS_ROUTES: Routes = [
  {
    path: '',
    component: QuestionsComponent,
  },
];
@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(QUESTIONS_ROUTES)
  ],
  declarations: [QuestionsComponent]
})
export class QuestionsModule { }
