import { Component, EventEmitter, HostBinding, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { MenuItem } from 'src/app/core/models/menu-item';
import { MenuService } from 'src/app/core/services/menu.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  host: {
    class: 'dvn-sidebar'
  }
})
export class SidebarComponent implements OnInit, OnDestroy {
  darkClassName = 'dark-theme';
  private routerSubscription!: Subscription;
  @Input() collapsed: boolean = false;
  @Input()
  theme!: string;
  @Output() collapsedChange = new EventEmitter<boolean>();
  @Output()
  onThemeSelected = new EventEmitter<string>()
  mainMenu: MenuItem[] = [];
  settingsMenu: MenuItem[] = [];
  constructor(private router: Router, private activatedRoute: ActivatedRoute, private menuService: MenuService) {

  }
  ngOnInit(): void {
    this.menuService.menu.subscribe(value => {
      this.mainMenu = value?.filter(m => m.isMainMenu) ?? [];
      this.settingsMenu = value?.filter(m => !m.isMainMenu) ?? [];
      this.activateSubMenu();
    })
    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.activateSubMenu();
      });
  }

  @HostBinding('class.sidebar-collapsed') get isCollapsed() { return this.collapsed; }
  public toggleSidebar() {
    this.collapsed = !this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }
  activateSubMenu() {
    this.activatedRoute.url.subscribe(r => {
      const url = this.router.url;
      this.mainMenu?.forEach(m => {
        if (m.subMenu) {
          m.isSubMenuActive = m.subMenu.some(s => s.route && url === ((s.route.startsWith('/') ? '' : '/') + s.route));
        }
      })
    })
  }
  toggleDarkMode() {
    const className = this.theme === this.darkClassName ? 'light-theme' : this.darkClassName;
    this.onThemeSelected.emit(className);
  }
  ngOnDestroy(): void {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }
}
