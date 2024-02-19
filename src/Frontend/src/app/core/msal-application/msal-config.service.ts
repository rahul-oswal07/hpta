import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BrowserCacheLocation, Configuration, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { map } from 'rxjs';

const isIE =
  window.navigator.userAgent.indexOf('MSIE ') > -1 ||
  window.navigator.userAgent.indexOf('Trident/') > -1;

@Injectable({
  providedIn: 'root'
})
export class MsalConfigService {
  private settings: any;
  private http: HttpClient;
  constructor(httpHandler: HttpBackend) {
    this.http = new HttpClient(httpHandler);
  }

  init(endpoint: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.get(window.location.origin + '/assets/' + endpoint).pipe(map(res => res))
        .subscribe(value => {
          this.settings = value;
          resolve(true);
        },
          (error) => {
            reject(error);
          });
    });
  }
  getClientApplication() {
    const msalConfig: Configuration = {
      auth: {
        clientId: this.getSettings('clientId'),
        authority: this.getSettings('authority'),
        redirectUri: this.getSettings('redirectUri'),
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
            // console.log(message);
          },
          logLevel: LogLevel.Verbose,
          piiLoggingEnabled: false,
        },
      },
    };
    return new PublicClientApplication(msalConfig);
  }
  getSettings(key?: string | Array<string>): any {
    if (!key || (Array.isArray(key) && !key[0])) {
      return this.settings;
    }

    if (!Array.isArray(key)) {
      key = key.split('.');
    }

    let result = key.reduce((acc: any, current: string) => acc && acc[current], this.settings);

    return result;
  }
}
