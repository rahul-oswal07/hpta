import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Params, Router } from '@angular/router';
import { BehaviorSubject, Subject, filter, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoutingHelperService {
  private routeParameterChangedSubject = new BehaviorSubject<Params | undefined>(undefined);
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.triggerInitialParameterCheck();

    // Listen to subsequent NavigationEnd events to handle navigation after the component loads
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute.firstChild;
        let child = route;

        while (child) {
          if (child.firstChild) {
            child = child.firstChild;
            route = child;
          } else {
            child = null;
          }
        }

        return route;
      }),
      filter(route => route?.outlet === 'primary'),
      map(route => route?.snapshot.params)
    ).subscribe(params => {
      this.routeParameterChangedSubject.next(params);
    });
  }

  private triggerInitialParameterCheck() {
    let currentRoute = this.activatedRoute.root;
    while (currentRoute.firstChild) {
      currentRoute = currentRoute.firstChild;
    }
    this.routeParameterChangedSubject.next(currentRoute.snapshot?.params);
  }

  public parameterChange() {
    this.triggerInitialParameterCheck();
    return this.routeParameterChangedSubject.asObservable();
  }
}
