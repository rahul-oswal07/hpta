import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MenuItem } from 'src/app/core/models/menu-item';
const basicMenu = [
  { id: '', name: 'Dashboard', route: '/', icon: 'home', canRead: true, isMainMenu: true },
  {
    id: '', name: 'Masters', route: 'master', icon: 'folder', canRead: true, isMainMenu: true, subMenu: [
      { id: '', name: 'Categories', route: 'categories', icon: 'category', canRead: true, isMainMenu: true },
      { id: '', name: 'Sub-Categories', route: 'subcategories', icon: 'subtitles', canRead: true, isMainMenu: true },
      { id: '', name: 'Questions', route: 'questions', icon: 'quiz', canRead: true, isMainMenu: true }
    ]
  },
  {
    id: '', name: 'Survey', route: 'surveys', icon: 'mood', canRead: true, isMainMenu: true, subMenu: [
      { id: '', name: 'Active Survey', route: 'survey/view', icon: 'summarize', canRead: true, isMainMenu: true }
    ]
  },
  { id: '', name: 'Results', route: 'result', icon: 'analytics', canRead: true, isMainMenu: true },
];
@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private menuSubject = new BehaviorSubject<MenuItem[]>([]);
  public menu = this.menuSubject.asObservable();
  constructor() { }

  reloadMenu() {
    this.menuSubject.next(basicMenu);
  }
}
