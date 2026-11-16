import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthResponseDto, LoginDto, RegisterDto } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // رابط الـ API الأساسي الخاص بالـ Back-End بناءً على الـ Swagger عندك
  private apiUrl = 'https://localhost:7053/api/Auth';

  constructor(private http: HttpClient) { }

  // دالة تسجيل الدخول
  login(model: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/login`, model);
  }

  // دالة إنشاء حساب جديد
  register(model: RegisterDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/register`, model);
  }
}