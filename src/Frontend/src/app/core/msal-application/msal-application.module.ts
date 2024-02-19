import { MsalConfigService } from './msal-config.service';
import { InjectionToken, NgModule, APP_INITIALIZER } from '@angular/core';
import {
  MsalService, MsalModule, MsalInterceptor, MSAL_INSTANCE, MsalInterceptorConfiguration, MsalGuardConfiguration, MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG, MsalGuard, MsalBroadcastService
} from '@azure/msal-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserCacheLocation, Configuration, IPublicClientApplication, InteractionType, LogLevel, PublicClientApplication } from '@azure/msal-browser';

const GRAPH_ENDPOINT = 'https://graph.microsoft.com/v1.0/me';
const AUTH_CONFIG_URL_TOKEN = new InjectionToken<string>('AUTH_CONFIG_URL');
const isIE =
  window.navigator.userAgent.indexOf('MSIE ') > -1 ||
  window.navigator.userAgent.indexOf('Trident/') > -1;

/**
* Scopes you add here will be prompted for user consent during sign-in.
* By default, MSAL.js will add OIDC scopes (openid, profile, email) to any login request.
* For more information about OIDC scopes, visit:
* https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
*/
// const loginRequest = {
//   scopes: ['email'],
// };

export function initializerFactory(env: MsalConfigService, configUrl: string): any {
  // APP_INITIALIZER, except a function return which will return a promise
  // APP_INITIALIZER, angular doesnt starts application untill it completes
  const promise = env.init(configUrl).then(() => {
    console.log(env.getSettings('clientId'));
  });
  return () => promise;
}

/**
 * Here we pass the configuration parameters to create an MSAL instance.
 * For more info, visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/configuration.md
 */

export function MSALInstanceFactory(config: MsalConfigService): IPublicClientApplication {
  const msalConfig: Configuration = {
    auth: {
      clientId: config.getSettings('clientId'),
      authority: config.getSettings('authority'),
      // knownAuthorities: [b2cPolicies.authorityDomain], // Mark your B2C tenant's domain as trusted.
      redirectUri: config.getSettings('redirectUri'),
      postLogoutRedirectUri: '/',
      navigateToLoginRequestUrl: true
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage, // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
      storeAuthStateInCookie: isIE, // Set this to "true" if you are having issues on IE11 or Edge
    },
    system: {
      allowNativeBroker: false, // Disables WAM Broker
      loggerOptions: {
        loggerCallback(logLevel: LogLevel, message: string) {
          console.log(message);
        },
        logLevel: LogLevel.Verbose,
        piiLoggingEnabled: false,
      },
    },
  };
  return new PublicClientApplication(msalConfig);
}

/**
 * MSAL Angular will automatically retrieve tokens for resources
 * added to protectedResourceMap. For more info, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/initialization.md#get-tokens-for-web-api-calls
 */
export function MSALInterceptorConfigFactory(config: MsalConfigService): MsalInterceptorConfiguration {
  const protectedResourceMap: Map<string, Array<string>> = new Map<string, Array<string>>();
  for (const [key, value] of Object.entries(config.getSettings('protectedResourceMap'))) {
    protectedResourceMap.set(key, value as string[]);
  }
  protectedResourceMap.set(GRAPH_ENDPOINT, ['user.read', 'user.readbasic.all']);
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap
  };
}

/**
 * Set your default interaction type for MSALGuard here. If you have any
 * additional scopes you want the user to consent upon login, add them here as well.
 */
export function MSALGuardConfigFactory(config: MsalConfigService): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: { scopes: config.getSettings('scopes') }
  };
}

@NgModule({
  declarations: [],
  imports: [
    MsalModule
  ]
})
export class MsalApplicationModule {
  static forRoot(configFile: string) {
    return {
      ngModule: MsalApplicationModule,
      providers: [
        MsalConfigService,
        { provide: AUTH_CONFIG_URL_TOKEN, useValue: configFile },
        {
          provide: APP_INITIALIZER, useFactory: initializerFactory,
          deps: [MsalConfigService, AUTH_CONFIG_URL_TOKEN], multi: true
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: MsalInterceptor,
          multi: true
        },
        {
          provide: MSAL_INSTANCE,
          useFactory: MSALInstanceFactory,
          deps: [MsalConfigService]
        },
        {
          provide: MSAL_GUARD_CONFIG,
          useFactory: MSALGuardConfigFactory,
          deps: [MsalConfigService]
        },
        {
          provide: MSAL_INTERCEPTOR_CONFIG,
          useFactory: MSALInterceptorConfigFactory,
          deps: [MsalConfigService]
        },
        MsalService,
        MsalGuard,
        MsalBroadcastService
      ]
    };
  }

}
