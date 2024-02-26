import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { finalize } from 'rxjs';
import { OTPRequest } from 'src/app/public-app/auth/models/otp-request';
import { OtpService } from 'src/app/public-app/auth/otp.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(2)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    otp: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(6)])
  });
  isOtpSent = false;
  isLoading = false;

  constructor(private otpService: OtpService, private dialogRef: MatDialogRef<LoginComponent>) {

  }
  get nameControl() {
    return this.form.get('name');
  }
  get emailControl() {
    return this.form.get('email');
  }

  get otpControl() {
    return this.form.get('otp');
  }

  ngOnInit(): void {

  }
  sendOtp() {
    if (this.emailControl?.valid) {
      const email = this.emailControl?.value!;
      const name = this.nameControl?.value!;
      // Call your service method to send OTP
      this.isLoading = true;
      this.otpService.sendOtp(name, email)
        .pipe(finalize(() => this.isLoading = false))
        .subscribe({
          next: (response) => {
            console.log(response);
            this.isOtpSent = true;
          },
          error: (error) => console.error(error),
        });
    }
  }

  verifyOtp() {
    if (this.form.valid) {
      const data = this.form.value as OTPRequest;
      // Call your service method to verify OTP and get JWT
      this.otpService.verifyOtp(data).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          this.dialogRef.close(response.token);
        },
        error: (error) => console.error(error)
      });
    }
  }
}
