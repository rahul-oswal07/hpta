import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EMPTY, Observable, from, map, switchMap, throwError } from 'rxjs';
import { AuthService } from 'src/app/public-app/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    if (request.url.indexOf('/anonymous/') === -1) {
      return this.addAuthorizationHeader(request, next);
    }
    return next.handle(request);
  }

  addAuthorizationHeader(req: HttpRequest<any>, next: HttpHandler) {
    return this.authService.getToken().pipe(switchMap((token: string | undefined) => {
      if (!!token) {
        const secureReq = req.clone({
          setHeaders: {
            Authorization: 'Bearer2 ' + token!,
            'X-Timezone-Offset': new Date().getTimezoneOffset().toString()
          }, withCredentials: false
        });
        return next.handle(secureReq);
      } else {
        const errorResponse = new HttpErrorResponse({
          error: 'Request cancelled by the interceptor',
          status: 0,
          statusText: 'Cancelled',
          url: req.url
        });
        return throwError(() => errorResponse);
      }
    }));
  }
}
