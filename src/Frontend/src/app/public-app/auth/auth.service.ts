import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map, of, Observable } from 'rxjs';
import { LoginComponent } from '../login/login.component';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private jwtService: JwtHelperService, private dialog: MatDialog) { }

  isLoggedIn(): Observable<boolean> {
    return this.getToken().pipe(map(r => !!r));
  }

  getToken() {
    let token = localStorage.getItem('token');
    if (!token || !this.validateToken(token)) {
      const dialogRef = this.dialog.open(LoginComponent);
      return dialogRef.afterClosed().pipe(map((token?: string) => {
        return token
      }));
    }
    return of(token);
  }

  private validateToken(token: string) {
    if (this.jwtService.isTokenExpired(token)) {
      localStorage.removeItem('token');
      return false;
    }
    return true;
  }
}
