import { Component, EventEmitter, HostBinding, Input, Output } from '@angular/core';
import { MenuItem } from 'src/app/core/models/menu-item';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  host: {
    class: 'dvn-sidebar'
  }
})
export class SidebarComponent {
  darkClassName = 'dark-theme';
  @Input() collapsed: boolean = false;
  @Input()
  theme!: string;
  @Output() collapsedChange = new EventEmitter<boolean>();
  @Output()
  onThemeSelected = new EventEmitter<string>()
  mainMenu: MenuItem[] = [];
  settingsMenu: MenuItem[] = [];
  private _menu?: MenuItem[] | undefined;
  public get menu(): MenuItem[] | undefined {
    return this._menu;
  }

  @Input()
  public set menu(value: MenuItem[] | undefined) {
    this._menu = value;
    this.mainMenu = value?.filter(m => m.isMainMenu) ?? [];
    this.settingsMenu = value?.filter(m => !m.isMainMenu) ?? [];
  }

  @HostBinding('class.sidebar-collapsed') get isCollapsed() { return this.collapsed; }
  public toggleSidebar() {
    this.collapsed = !this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }

  toggleDarkMode() {
    const className = this.theme === this.darkClassName ? 'light-theme' : this.darkClassName;
    this.onThemeSelected.emit(className);
  }
}
