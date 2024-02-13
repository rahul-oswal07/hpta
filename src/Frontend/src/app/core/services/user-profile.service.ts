import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, of, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(private http: HttpClient) { }

  getUserPhoto() {
    return this.http.get('https://graph.microsoft.com/v1.0/me/photos').pipe(switchMap(r => {
      console.log(r);
      return this.http.get('https://graph.microsoft.com/v1.0/me/photos/48x48/$value', { responseType: 'blob' });
    }), catchError(e => {

      return of(null)
    }))
    // const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getUserProfile() {
    return this.http.get('https://graph.microsoft.com/v1.0/me');
  }
}
