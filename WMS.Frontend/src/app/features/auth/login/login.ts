import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/login-request';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  loginData: LoginRequest = {
    username: '',
    password: '',
  };
  
  errorMessage: string = '';
  isLoading = false;

  ngOnInit(): void{
    if(this.authService.isLoggedIn()){
      this.router.navigate(['/dashboard']);
    }
  }

  onLogin(): void{
    if(this.isLoading) return;

    this.errorMessage = '';
    this.isLoading = true;
    
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        this.authService.saveAuthData(response);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        if (err.status === 401) {
          this.errorMessage = 'Invalid username or password';
        } else {
          this.errorMessage = 'Something went wrong';
        }

        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
