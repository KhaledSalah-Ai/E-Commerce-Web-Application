import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    // إعداد الحقول وقواعد التحقق (Validation)
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  // دالة تُنفذ عند الضغط على زر تسجيل الدخول
  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Login Success:', response);
          // حفظ التوكن في الـ LocalStorage
          localStorage.setItem('token', response.token);
          
          // الانتقال الفوري للصفحة الرئيسية
          this.router.navigate(['/home']);
        },
        error: (err) => {
          console.error('Login Error:', err);
          this.errorMessage = err.error?.message || 'حدث خطأ أثناء تسجيل الدخول، تأكد من البيانات.';
        }
      });
    }
  }
}