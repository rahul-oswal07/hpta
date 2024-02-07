import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ViewSurveyComponent } from './view-survey/view-survey.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatExpansionModule } from '@angular/material/expansion';
import { ReactiveFormsModule } from '@angular/forms';
import { RatingModule } from 'src/app/modules/rating/rating.module';
import { MatButtonModule } from '@angular/material/button';
import { DataStatusIndicatorModule } from 'src/app/modules/data-status-indicator/data-status-indicator.module';
import { SurveyResultComponent } from './survey-result/survey-result.component';
import { CreateSurveyComponent } from './create-survey/create-survey.component';

const SURVEY_ROUTES: Routes = [
  {
    path: 'view/:id',
    component: ViewSurveyComponent,
  },
  {
    path: 'results/:id',
    component: SurveyResultComponent,
  },
];
@NgModule({
  declarations: [
    ViewSurveyComponent,
    SurveyResultComponent,
    CreateSurveyComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatExpansionModule,
    MatButtonModule,
    RatingModule,
    DataStatusIndicatorModule,
    RouterModule.forChild(SURVEY_ROUTES)
  ],
})
export class SurveyModule { }
