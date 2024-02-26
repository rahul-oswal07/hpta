import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MSAL_GUARD_CONFIG, MsalGuardConfiguration, MsalService } from '@azure/msal-angular';
import { AccountInfo, AuthenticationResult, PopupRequest, PublicClientApplication, RedirectRequest } from '@azure/msal-browser';
import { Subject } from 'rxjs';
import { MenuService } from 'src/app/core/services/menu.service';
import { UserProfileService } from 'src/app/core/services/user-profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  accountInfo: AccountInfo | null = null;
  avatarUrl?: SafeUrl;
  isIframe = false;
  theme = '';
  loginDisplay = false;
  sidebarCollapsed = false;
  private readonly _destroying$ = new Subject<void>();
  constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private readonly renderer: Renderer2,
    private profileService: UserProfileService,
    private menuService: MenuService,
    private domSanitizer: DomSanitizer,
    @Inject(DOCUMENT) private readonly document: Document) {

  }
  ngOnInit(): void {
    this.menuService.reloadMenu();
    this.setActiveAccount();
  }

  setActiveAccount() {
    this.accountInfo = this.authService.instance.getActiveAccount() ?? this.authService.instance.getAllAccounts()[0];
    if (this.accountInfo) {
      this.profileService.getUserPhoto().subscribe((r: Blob | null) => {
        if (r) {
          const urlCreator = window.URL || window.webkitURL;
          const pic = this.domSanitizer.bypassSecurityTrustUrl(urlCreator.createObjectURL(r))
          this.avatarUrl = pic;
        } else {
          this.avatarUrl = undefined;
        }
      })
    }
    // console.log(this.accountInfo);
  }

  loginRedirect() {
    if (this.msalGuardConfig.authRequest) {
      this.authService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
    } else {
      this.authService.loginRedirect();
    }
  }

  loginPopup() {
    if (this.msalGuardConfig.authRequest) {
      this.authService.loginPopup({ ...this.msalGuardConfig.authRequest } as PopupRequest)
        .subscribe((response: AuthenticationResult) => {
          this.authService.instance.setActiveAccount(response.account);
        });
    } else {
      let msalInstance: PublicClientApplication = this.authService.instance as PublicClientApplication;
      msalInstance.clearCache();
      this.authService.loginPopup()
        .subscribe((response: AuthenticationResult) => {
          this.authService.instance.setActiveAccount(response.account);
        });
    }
  }
  onThemeSelected(newTheme: string) {
    localStorage.setItem('theme', newTheme);
    this.applyTheme();
  }

  applyTheme() {
    this.theme = localStorage.getItem('theme') ?? 'light-theme';
    const hostElement = this.document.body;
    this.renderer.removeClass(hostElement, 'dark-theme');
    this.renderer.removeClass(hostElement, 'light-theme');
    this.renderer.addClass(hostElement, this.theme);
    if (this.theme === 'dark-theme') {
      this.document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  logout(popup?: boolean) {
    if (popup) {
      this.authService.logoutPopup({
        mainWindowRedirectUri: "/"
      });
    } else {
      this.authService.logoutRedirect();
    }
  }

  ngOnDestroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
  }

}
