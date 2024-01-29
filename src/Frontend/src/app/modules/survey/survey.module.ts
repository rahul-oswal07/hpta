import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ViewSurveyComponent } from './view-survey/view-survey.component';
const SURVEY_ROUTES: Routes = [
  {
    path: ':id',
    component: ViewSurveyComponent,
  },
];
@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(SURVEY_ROUTES)],
})
export class SurveyModule {}
