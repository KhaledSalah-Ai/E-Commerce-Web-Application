// نموذج استجابة تسجيل الدخول (يتضمن الـ Token واسم المستخدم مثلاً حسب الـ Back-End عندك)
export interface AuthResponseDto {
  token: string;
  email: string;
  message?: string;
  isSuccess: boolean;
}

// نموذج بيانات تسجيل الدخول
export interface LoginDto {
  email: string;
  password: string;
}

// نموذج بيانات إنشاء حساب جديد
export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber: string;
}