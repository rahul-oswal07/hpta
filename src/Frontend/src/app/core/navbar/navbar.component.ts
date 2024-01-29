import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { AccountInfo } from '@azure/msal-browser';

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
  title: string = '';

  @Input()
  notificationCount?: number;

  @Input()
  loggedInUser: AccountInfo | null = null;

  @Input()
  isMobile = false;

  @Input()
  loginDisplay = false;



  @Output()
  onSearch = new EventEmitter<string>();

  @Output()
  loginClick = new EventEmitter();

  @Output()
  logoutClick = new EventEmitter();

  ngOnInit() {
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
