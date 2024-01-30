import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ViewSurveyComponent } from './view-survey/view-survey.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatExpansionModule } from '@angular/material/expansion';
import { ReactiveFormsModule } from '@angular/forms';
import { RatingModule } from 'src/app/modules/rating/rating.module';
import { MatButtonModule } from '@angular/material/button';

const SURVEY_ROUTES: Routes = [
  {
    path: ':id',
    component: ViewSurveyComponent,
  },
];
@NgModule({
  declarations: [
    ViewSurveyComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatExpansionModule,
    MatButtonModule,
    RatingModule,
    RouterModule.forChild(SURVEY_ROUTES)
  ],
})
export class SurveyModule { }
