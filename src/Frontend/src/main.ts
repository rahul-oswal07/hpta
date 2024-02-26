import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/default-app/app.module';
import { Configuration, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalGuardConfiguration, MsalInterceptorConfiguration } from '@azure/msal-angular';
import { PublicAppModule } from 'src/app/public-app/public-app.module';
import { APP_BASE_HREF } from '@angular/common';

function loggerCallback(logLevel: LogLevel, message: string) {
  console.log("MSAL Angular: ", message);
}
function checkAndSetActiveAccount(instance: PublicClientApplication) {
  let activeAccount = instance.getActiveAccount();

  if (!activeAccount && instance.getAllAccounts().length > 0) {
    let accounts = instance.getAllAccounts();
    instance.setActiveAccount(accounts[0]);
  }
}

// function setBaseHref(baseHref: string) {
//   const baseElements = document.getElementsByTagName('base');
//   if (baseElements.length) {
//     baseElements[0].setAttribute('href', baseHref);
//   } else {
//     // If no base element exists, create one and append it to the head
//     const base = document.createElement('base');
//     base.setAttribute('href', baseHref);
//     document.head.appendChild(base);
//   }
// }
const pathName = window.location.pathname;
if (pathName.startsWith('/guest')) {
  const providers = [
    { provide: APP_BASE_HREF, useValue: '/guest/' },
  ];
  // setBaseHref('/public/');
  platformBrowserDynamic(providers).bootstrapModule(PublicAppModule)
    .catch(err => console.error(err));

} else {
  fetch('/assets/config.json')
    .then(response => response.json())
    .then(async json => {

      const msalConfig: Configuration = {
        auth: {
          clientId: json.msal.auth.clientId,
          authority: json.msal.auth.authority,
          redirectUri: window.location.origin,
          postLogoutRedirectUri: window.location.origin,
          navigateToLoginRequestUrl: json.msal.auth.navigateToLoginRequestUrl
        },
        cache: json.msal.cache,
        system: {
          allowNativeBroker: false, // Disables WAM Broker
          loggerOptions: {
            loggerCallback,
            logLevel: LogLevel.Error,
            piiLoggingEnabled: false,
          },
        }
      };
      const msalInstance = new PublicClientApplication(msalConfig);
      await msalInstance.initialize();
      msalInstance.handleRedirectPromise().then(authResult => {
        if (authResult) {
          // Authenticated successfully, authResult contains the authentication response
          console.log('Authentication result', authResult);
        } else {
          // Not authenticated, check if there are any accounts
          const accounts = msalInstance.getAllAccounts();
          if (accounts.length === 0) {
            // No user is signed in, initiate sign-in
            msalInstance.loginRedirect();
            return; // Important to return here to prevent bootstrapping the app before redirect
          } else {
            checkAndSetActiveAccount(msalInstance);
          }
        }
        // Proceed to bootstrap the Angular application
        platformBrowserDynamic([
          { provide: APP_BASE_HREF, useValue: '/' },
          {
            provide: MSAL_INSTANCE, useValue: msalInstance
          },
          {
            provide: MSAL_GUARD_CONFIG, useValue: {
              interactionType: json.guard.interactionType,
              authRequest: json.guard.authRequest,
              loginFailedRoute: json.guard.loginFailedRoute
            } as MsalGuardConfiguration
          },
          {
            provide: MSAL_INTERCEPTOR_CONFIG, useValue: {
              interactionType: json.interceptor.interactionType,
              protectedResourceMap: new Map(json.interceptor.protectedResourceMap)
            } as MsalInterceptorConfiguration
          },
        ]).bootstrapModule(AppModule)
          .catch(err => console.error(err));
      }).catch(error => {
        // Handle errors occurred during the redirect
        console.error(error);
      });

    })

  // platformBrowserDynamic().bootstrapModule(AppModule)
  //   .catch(err => console.error(err));

}
