import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Configuration } from '@azure/msal-browser';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  private configUrl = 'api/config'; // URL to web api

  constructor(private http: HttpClient) { }

  getConfig(): Observable<Configuration> {
    return this.http.get<Configuration>(this.configUrl);
  }
}
