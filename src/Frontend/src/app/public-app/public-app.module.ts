import { NgModule } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { PublicAppComponent } from './public-app.component';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/core/shared/shared.module';
import { PublicHomeComponent } from './public-home/public-home.component';
import { LoginComponent } from './login/login.component';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { authGuard } from 'src/app/public-app/auth/auth.guard';
import { MatButtonModule } from '@angular/material/button';
import { AuthInterceptorService } from 'src/app/public-app/auth/auth.interceptor.service';
import { MatDialogModule } from '@angular/material/dialog';
import { AuthService } from 'src/app/public-app/auth/auth.service';
import { JwtModule } from '@auth0/angular-jwt';

export function getToken(): string | null {
  return localStorage.getItem('token');
}

const routes: Routes = [
  {
    path: '',
    redirectTo: 'survey/view/1',
    pathMatch: 'full'
  },
  {
    path: 'survey',
    title: 'HPTA | Survey',
    loadChildren: () =>
      import('../modules/survey/survey.module').then((m) => m.SurveyModule),
  },
  {
    path: 'result',
    title: 'HPTA | Result',
    canActivate: [authGuard],
    loadChildren: () =>
      import('../modules/survey-result/survey-result.module').then((m) => m.SurveyResultModule),
  },
];


@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: getToken
      }
    }),
    RouterModule.forRoot(routes)
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline', subscriptSizing: 'dynamic' } }
  ],
  declarations: [PublicAppComponent, PublicHomeComponent, LoginComponent],
  bootstrap: [PublicAppComponent]
})
export class PublicAppModule { }
