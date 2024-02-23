import { CommonModule } from "@angular/common";
import { SurveyResultComponent } from "./survey-result.component";
import { ReactiveFormsModule } from "@angular/forms";
import { MatStepperModule } from "@angular/material/stepper";
import { MatExpansionModule } from "@angular/material/expansion";
import { MatButtonModule } from "@angular/material/button";
import { DataStatusIndicatorModule } from "../data-status-indicator/data-status-indicator.module";
import { SurveyResultRoutingModule } from "./survey-result-routing.module";
import { NgModule } from "@angular/core";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { MatInputModule } from "@angular/material/input";
import { NgApexchartsModule } from 'ng-apexcharts';
import { SurveyResultDetailsComponent } from "src/app/modules/survey-result/survey-result-details/survey-result-details.component";

@NgModule({
  declarations: [
    SurveyResultComponent, SurveyResultDetailsComponent
  ],
  imports: [
    CommonModule,
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
  ]
})
export class SurveyResultModule { }
