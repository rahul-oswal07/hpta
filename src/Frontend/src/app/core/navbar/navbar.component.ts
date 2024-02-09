import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRouteSnapshot, Router, RoutesRecognized } from '@angular/router';
import { AccountInfo } from '@azure/msal-browser';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  host: {
    class: 'dvn-navbar',
    '[class]': 'color ? "mat-" + color : ""'
  }
})
export class NavbarComponent implements OnInit {
  @Input() color?: ThemePalette | null;

  @Input()
  notificationCount?: number;

  @Input()
  loggedInUser: AccountInfo | null = null;

  @Input()
  isMobile = false;

  @Input()
  loginDisplay = false;

  @Input()
  avatarUrl?: SafeUrl;

  @Output()
  onSearch = new EventEmitter<string>();

  @Output()
  loginClick = new EventEmitter();

  @Output()
  logoutClick = new EventEmitter();
  private appTitle = 'HPTA';
  title = this.appTitle;
  constructor(private router: Router) { }
  ngOnInit() {
    this.router.events.pipe(filter((event: any) => event instanceof RoutesRecognized),
      map((event: RoutesRecognized) => {
        return event?.state?.root?.firstChild?.data || { title: '' };
      }))
      .subscribe(customData => {
        const title = customData['title']
        this.title = this.appTitle + (title ? (' | ' + title) : '');
      });
  }
  getTitleRecursively(node: ActivatedRouteSnapshot | any, title: string): string {
    if (node?.children?.length > 0) {
      return this.getTitleRecursively(node?.firstChild, title);
    }
    if (node.data.title) {
      title += ' | ' + node.data.title;
    }
    if (node.params.title) {
      title += ' | ' + node.params.title;
    }
    return title;
  }
  performSearch(event: Event) {
    const keywords = (event.target as HTMLInputElement).value;
    this.onSearch.emit(keywords);
  }

  logout() {
    this.logoutClick.emit();
  }
  login() {
    this.loginClick.emit();
  }


}
