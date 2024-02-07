import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserConfigService {

  constructor(private httpClient: HttpClient) { }

  getUserConfig() {
    return this.httpClient.post('api/user/config', null);
  }
}
