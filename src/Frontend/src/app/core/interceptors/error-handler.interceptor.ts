import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerInterceptor implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Unknown error occurred!';
        if (error.error instanceof ErrorEvent) {
          // Client-side errors
          errorMessage = error.error.message;
        } else {
          // Server-side errors
          errorMessage = error.message;
        }
        console.error(errorMessage);

        // Propagate the error
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
