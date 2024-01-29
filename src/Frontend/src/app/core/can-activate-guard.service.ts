import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CanActivateGuard implements CanActivate {
  constructor(private router: Router) {
  }

  /**
   * Test for user to be logged in and allow navigation
   * @param route
   * @param state
   */
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    if (this.isLoggedIn()) {
      return true;
    } else {
      window.location.assign('/account/logout');
      return false;
    }
  }

  /**
   * Return true if user is logged in
   */
  isLoggedIn(): boolean {
    // TODO: should we validate authId here? ot let BE do all the validation?
    return localStorage.getItem('access_token') ? true : false;
  }
}
