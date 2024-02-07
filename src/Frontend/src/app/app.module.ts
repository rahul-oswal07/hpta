import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppConfigService } from 'src/app/core/services/app-config.service';
import { initializeApp } from 'src/app/core/utils/initialize-app';
import { MsalApplicationModule } from 'src/app/core/msal-application/msal-application.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { BrowserCacheLocation, IPublicClientApplication, InteractionType, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalBroadcastService, MsalGuard, MsalGuardConfiguration, MsalInterceptor, MsalInterceptorConfiguration, MsalModule, MsalRedirectComponent, MsalService } from '@azure/msal-angular';
import { environment } from 'src/environments/environment.prod';
import { FailedLoginComponent } from './failed-login/failed-login.component';
import { ProfileComponent } from './profile/profile.component';
import { SidebarComponent } from 'src/app/core/sidebar/sidebar.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NavbarComponent } from 'src/app/core/navbar/navbar.component';

export const APP_DATE_FORMATS = {
  parse: {
    dateInput: 'yyyy-MM-DD HH:mm:ss'
  },
  display: {
    dateInput: 'dd/MMM/yyyy',
    monthYearLabel: 'dd MMM yyyy',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'dd MMMM yyyy'
  }
};
export const APP_MONTH_YEAR_FORMATS = {
  parse: {
    dateInput: 'MMM/yyyy'
  },
  display: {
    dateInput: 'MMM/yyyy',
    monthYearLabel: 'MMM yyyy',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM yyyy'
  }
};

export function loggerCallback(logLevel: LogLevel, message: string) {
  console.log(message);
}

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.msalConfig.auth.clientId,
      authority: environment.msalConfig.auth.authority,
      navigateToLoginRequestUrl: true,
      redirectUri: '/',
      postLogoutRedirectUri: '/',

    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
      storeAuthStateInCookie: true
    },
    system: {
      allowNativeBroker: false, // Disables WAM Broker
      loggerOptions: {
        loggerCallback,
        logLevel: LogLevel.Info,
        piiLoggingEnabled: false
      }
    }
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  // protectedResourceMap.set(environment.apiConfig.uri, environment.apiConfig.scopes);
  protectedResourceMap.set(
    'api/*',
    ['https://devonhpta.onmicrosoft.com/14d3b6e3-2d3c-40cc-9099-fd81b654394f/Survey.Read']
  );
  protectedResourceMap.set(
    'https://graph.microsoft.com/v1.0/me',
    ['User.Read', 'User.ReadBasic.All']
  );

  return {
    interactionType: InteractionType.Popup,
    protectedResourceMap
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      // scopes: [...environment.apiConfig.scopes]
    },
    loginFailedRoute: '/login-failed'
  };
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FailedLoginComponent,
    ProfileComponent,
    SidebarComponent,
    NavbarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    // MsalApplicationModule.forRoot('config.json'),
    MatToolbarModule,
    MatMenuModule,
    MsalModule,
    MatIconModule,
    MatTooltipModule
  ],
  // providers: [
  //   AppConfigService,
  // {
  //   provide: APP_INITIALIZER,
  //   useFactory: initializeApp,
  //   deps: [AppConfigService],
  //   multi: true
  // }
  // ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalGuard,
    MsalBroadcastService
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
