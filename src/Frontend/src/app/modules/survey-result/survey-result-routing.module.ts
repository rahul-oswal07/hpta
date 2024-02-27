import { Router, RouterModule, Routes } from "@angular/router";
import { SurveyResultComponent } from "./survey-result.component";
import { Inject, Injectable, NgModule } from "@angular/core";
import { SurveyResultDetailsComponent } from "src/app/modules/survey-result/survey-result-details/survey-result-details.component";
import { APP_BASE_HREF } from "@angular/common";

export function configSurveyResultRoutes() {
  let routes = [];
  const pathName = window.location.pathname;
  if (pathName.startsWith('/guest')) {
    routes = publicRoutes;
  } else {
    routes = defaultRoutes;
  }
  return routes;
}

const defaultRoutes: Routes = [
  {
    path: '',
    component: SurveyResultComponent,
    children: [
      {
        path: 'team/:id',
        component: SurveyResultDetailsComponent
      }
    ]
  }
];

const publicRoutes: Routes = [
  {
    path: '',
    component: SurveyResultDetailsComponent
  }
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(configSurveyResultRoutes())],
  exports: [RouterModule]
})
export class SurveyResultRoutingModule { }
