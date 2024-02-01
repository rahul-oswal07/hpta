import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, of, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(private http: HttpClient) { }

  getUserPhoto() {
    return this.http.get('https://graph.microsoft.com/v1.0/me/photo').pipe(switchMap(r => {
      console.log(r);
      return this.http.get('https://graph.microsoft.com/v1.0/me/photo/$value', { responseType: 'blob' });
    }), catchError(e => {
      console.log(e);
      const svg = `

      <svg width="42px" height="42px" viewBox="-5 -5 42 42"
    xmlns="http://www.w3.org/2000/svg">
    <circle cx="16" cy="16" r="20" fill="none" stroke="#000000" stroke-width="2"/>
        <circle cx="16" cy="10" r="10" fill="none" stroke="#000000" stroke-width="2"/>
    <path d="M31.537,26.679a.991.991,0,0,1-.676-.264,21.817,21.817,0,0,0-29.2-.349,1,1,0,1,1-1.322-1.5,23.814,23.814,0,0,1,31.875.377,1,1,0,0,1-.677,1.736Z"></path>
</svg>`;
      const blob = new Blob([svg], { type: 'image/svg+xml' });
      return of(blob)
    }))
    // const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getUserProfile() {
    return this.http.get('https://graph.microsoft.com/v1.0/me');
  }
}
