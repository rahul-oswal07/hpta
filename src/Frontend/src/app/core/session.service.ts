import { Injectable } from '@angular/core';

// warnings before session expiration in minutes
export const firstExpirationWarningMinutes = 30;
export const finalExpirationWarningMinutes = 10;
@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private redirectAfterSessionExpirationDisabled = false;

  private set expiresAt(val) {
      localStorage.setItem('expires_at', val);
  }

  private get expiresAt() {
      // .NET core wants to put . instead of :
      return localStorage.getItem('expires_at')?.replace(/\./g, ':') || '00:00:00';
  }

  constructor() {

  }

  public async startTokenValidityChecker(): Promise<void> {

      const expires = +new Date(this.expiresAt);
      const ubiSessionExpiresInMilliseconds = expires - Date.now();
      const firstWarning = expires - (firstExpirationWarningMinutes * 60 * 1000);
      const finalWarning = expires - (finalExpirationWarningMinutes * 60 * 1000);

      const firstWarningTime = ubiSessionExpiresInMilliseconds - (firstExpirationWarningMinutes * 60 * 1000);
      const finalWarningTime = ubiSessionExpiresInMilliseconds - (finalExpirationWarningMinutes * 60 * 1000);

      console.log('session expires at ' + new Date(expires));
      console.log('setting session expiration timeouts');
      console.log('first warning at ' + new Date(firstWarning));
      console.log('final warning at ' + new Date(finalWarning));

      const cfg = { enableHtml: true, closeButton: true, disableTimeOut: true };

      const debugMessage = 'session expiring at ' + new Date(expires);

      const warningTitle = 'Session expiring';
      const firstWarningMessage = 'Session is expiring in {{timeoutMinutes}} minutes. Save changes.';
      const finalWarningMessage = 'Session is expiring in {{timeoutMinutes}} minutes. Save changes.';

      const expiredTitle = 'Session expired';
      const sessionExpiredMessage = 'Session has expired. Reload the page.';

      // Show the first warning about session expiration
      setTimeout(
          () => {
              // this.ngxNotificationService.getWarningAlert(firstWarningMessage, cfg, warningTitle);
              return console.log(debugMessage);
          },
          firstWarningTime < 0 ? 0 : firstWarningTime
      );

      // Show the final warning about session expiration
      setTimeout(
          () => {
              // this.ngxNotificationService.getWarningAlert(finalWarningMessage, cfg, warningTitle);
              return console.log(debugMessage);
          },
          finalWarningTime < 0 ? 0 : finalWarningTime
      );

      // Redirect browser to login
      setTimeout(
          () => {
              // this.ngxNotificationService.getErrorAlert(sessionExpiredMessage, cfg, expiredTitle);

              console.log('redirecting to login page');

              if (!this.redirectAfterSessionExpirationDisabled) {
                  window.location.href = `/account/login?returnUrl=${encodeURIComponent(window.location.href)}`;
              }

          },
          ubiSessionExpiresInMilliseconds < 0 ? 0 : ubiSessionExpiresInMilliseconds
      );
  }
}
