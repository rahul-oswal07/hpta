import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OTPRequest } from 'src/app/public-app/auth/models/otp-request';
import { TokenResponse } from 'src/app/public-app/auth/models/token-response';

@Injectable({
  providedIn: 'root'
})
export class OtpService {
  private http: HttpClient;
  constructor(handler: HttpBackend) {
    this.http = new HttpClient(handler);
  }

  sendOtp(name: string, email: string): Observable<any> {
    return this.http.post('/api/anonymous/otp/send', { name, email });
  }

  verifyOtp(data: OTPRequest): Observable<TokenResponse> {
    return this.http.post<TokenResponse>('/api/anonymous/otp/verify', data);
  }

}
