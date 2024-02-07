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
    canActivate: [MsalGuard],
  },
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'login-failed',
    component: FailedLoginComponent,
  },
  {
    path: 'survey',
    loadChildren: () =>
      import('./modules/survey/survey.module').then((m) => m.SurveyModule),
  }, {
    path: 'categories',
    loadChildren: () =>
      import('./modules/categories/categories.module').then((m) => m.CategoriesModule),
  }, {
    path: 'subcategories',
    loadChildren: () =>
      import('./modules/sub-categories/sub-categories.module').then((m) => m.SubCategoriesModule),
  }, {
    path: 'questions',
    loadChildren: () =>
      import('./modules/questions/questions.module').then((m) => m.QuestionsModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
