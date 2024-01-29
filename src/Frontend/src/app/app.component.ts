import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService } from '@azure/msal-angular';
import { AccountInfo, AuthenticationResult, EventMessage, EventType, IPublicClientApplication, InteractionStatus, InteractionType, PopupRequest, PublicClientApplication, RedirectRequest } from '@azure/msal-browser';
import { Observable, Subject, filter, map, of, takeUntil } from 'rxjs';
import { MenuItem } from 'src/app/core/models/menu-item';
import { UserProfileService } from 'src/app/core/services/user-profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'HPTA';
  accountInfo: AccountInfo | null = null;
  isIframe = false;
  theme = '';
  loginDisplay = false;
  sidebarCollapsed = false;
  menu!: Observable<MenuItem[]>;
  private readonly _destroying$ = new Subject<void>();
  constructor(@Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private readonly renderer: Renderer2,
    @Inject(DOCUMENT) private readonly document: Document) {

  }
  ngOnInit(): void {
    this.menu = of([
      { id: '', name: 'Dashboard', route: '', icon: 'home', canRead: true, isMainMenu: true },
      {
        id: '', name: 'Masters', route: 'master', icon: 'folder', canRead: true, isMainMenu: true, subMenu: [
          { id: '', name: 'Categories', route: 'categories', icon: 'category', canRead: true, isMainMenu: true },
          { id: '', name: 'Sub-Categories', route: 'subcategories', icon: 'subtitles', canRead: true, isMainMenu: true },
          { id: '', name: 'Questions', route: 'questions', icon: 'quiz', canRead: true, isMainMenu: true }
        ]
      },
      { id: '', name: 'Survey', route: 'surveys', icon: 'mood', canRead: true, isMainMenu: true }
    ])
    /**
     * You can subscribe to MSAL events as shown below. For more info,
     * visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-angular/docs/v2-docs/events.md
     */
    this.isIframe = window !== window.parent && !window.opener; // Remove this line to use Angular Universal
    this.setLoginDisplay();

    this.authService.instance.enableAccountStorageEvents(); // Optional - This will enable ACCOUNT_ADDED and ACCOUNT_REMOVED events emitted when a user logs in or out of another tab or window
    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED),
      )
      .subscribe((result: EventMessage) => {
        if (this.authService.instance.getAllAccounts().length === 0) {
          window.location.pathname = "/";
        } else {
          this.setLoginDisplay();
        }
      });

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this._destroying$)
      )
      .subscribe(() => {
        this.setLoginDisplay();
        this.checkAndSetActiveAccount();
      })
  }
  setLoginDisplay() {
    this.loginDisplay = this.authService.instance.getAllAccounts().length === 0;
  }
  checkAndSetActiveAccount() {
    /**
     * If no active account set but there are accounts signed in, sets first account to active account
     * To use active account set here, subscribe to inProgress$ first in your component
     * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
     */
    let activeAccount = this.authService.instance.getActiveAccount();

    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      let accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
    }
    this.accountInfo = this.authService.instance.getActiveAccount();
    // if (this.accountInfo) {

    //   this.profileService.getUserPhoto().subscribe(r => {
    //     console.log(r);
    //   })
    // }
    console.log(this.accountInfo);
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
