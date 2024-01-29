import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(private http: HttpClient) { }

  getUserPhoto() {
    // const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get('https://graph.microsoft.com/v1.0/me/photo/$value', { responseType: 'blob' });
  }
}
