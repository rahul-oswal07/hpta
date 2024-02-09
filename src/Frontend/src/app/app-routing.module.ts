import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { FailedLoginComponent } from 'src/app/failed-login/failed-login.component';
import { HomeComponent } from 'src/app/home/home.component';
import { ProfileComponent } from 'src/app/profile/profile.component';

const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    data: { title: 'Profile Information' },
    canActivate: [MsalGuard],
  },
  {
    path: '',
    component: HomeComponent,
    data: { title: 'Dashboard' }
  },
  {
    path: 'login-failed',
    data: { title: 'Failed Login' },
    component: FailedLoginComponent,
  },
  {
    path: 'survey',
    data: { title: 'Survey' },
    loadChildren: () =>
      import('./modules/survey/survey.module').then((m) => m.SurveyModule),
  },
  {
    path: 'categories',
    data: { title: 'Categories' },
    loadChildren: () =>
      import('./modules/categories/categories.module').then((m) => m.CategoriesModule),
  },
  {
    path: 'subcategories',
    data: { title: 'Sub-Categories' },
    loadChildren: () =>
      import('./modules/sub-categories/sub-categories.module').then((m) => m.SubCategoriesModule),
  },
  {
    path: 'questions',
    data: { title: 'Questions' },
    loadChildren: () =>
      import('./modules/questions/questions.module').then((m) => m.QuestionsModule),
  },
  {
    path: 'result',
    loadChildren: () =>
      import('./modules/survey-result/survey-result.module').then((m) => m.SurveyResultModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
