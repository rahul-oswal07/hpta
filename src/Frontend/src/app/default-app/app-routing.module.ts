import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { FailedLoginComponent } from 'src/app/default-app/failed-login/failed-login.component';
import { HomeComponent } from 'src/app/default-app/home/home.component';
import { ProfileComponent } from 'src/app/default-app/profile/profile.component';

const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    title: 'HPTA | Profile Information',
    canActivate: [MsalGuard],
  },
  {
    path: '',
    component: HomeComponent,
    title: 'HPTA | Dashboard'
  },
  {
    path: 'login-failed',
    title: 'HPTA | Failed Login',
    component: FailedLoginComponent,
  },
  {
    path: 'survey',
    title: 'HPTA | Survey',
    loadChildren: () =>
      import('../modules/survey/survey.module').then((m) => m.SurveyModule),
  },
  {
    path: 'categories',
    title: 'HPTA | Categories',
    loadChildren: () =>
      import('../modules/categories/categories.module').then((m) => m.CategoriesModule),
  },
  {
    path: 'subcategories',
    title: 'HPTA | Sub-Categories',
    loadChildren: () =>
      import('../modules/sub-categories/sub-categories.module').then((m) => m.SubCategoriesModule),
  },
  {
    path: 'questions',
    title: 'HPTA | Questions',
    loadChildren: () =>
      import('../modules/questions/questions.module').then((m) => m.QuestionsModule),
  },
  {
    path: 'result',
    title: 'HPTA | Result',
    loadChildren: () =>
      import('../modules/survey-result/survey-result.module').then((m) => m.SurveyResultModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
