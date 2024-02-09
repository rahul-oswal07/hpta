import { RouterModule, Routes } from "@angular/router";
import { SurveyResultComponent } from "./survey-result.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
  {
    path: '',
    component: SurveyResultComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SurveyResultRoutingModule { }