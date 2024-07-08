import { CommonModule } from '@angular/common';
import { SurveyResultComponent } from './survey-result.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatStepperModule } from '@angular/material/stepper';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { DataStatusIndicatorModule } from '../data-status-indicator/data-status-indicator.module';
import { SurveyResultRoutingModule } from './survey-result-routing.module';
import { NgModule } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { NgApexchartsModule } from 'ng-apexcharts';
import { SurveyResultDetailsComponent } from 'src/app/modules/survey-result/survey-result-details/survey-result-details.component';
import { MatIconModule } from '@angular/material/icon';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatChipsModule } from '@angular/material/chips';
import { RatingModule } from 'src/app/modules/rating/rating.module';
import { MatCardModule } from '@angular/material/card';
import {MatTabsModule} from '@angular/material/tabs';
import {MatProgressBarModule} from '@angular/material/progress-bar';


@NgModule({
  declarations: [SurveyResultComponent, SurveyResultDetailsComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatExpansionModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    DataStatusIndicatorModule,
    SurveyResultRoutingModule,
    NgApexchartsModule,
    MatIconModule,
    MatChipsModule,
    RatingModule,
    NgxMatSelectSearchModule,
    MatCardModule,
    MatTabsModule,
    MatProgressBarModule
  ],
})
export class SurveyResultModule {}
