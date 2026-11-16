import { Routes } from '@angular/router';
import { Home } from './components/home/home'; // تأكد من اسم ومسار مكون الـ Home عندك
import { LoginComponent } from './components/login/login';
import { Register } from './components/register/register';
import { About } from './components/about/about';
import { Contact } from './components/contact/contact';
import { Privacy } from './components/privacy/privacy';
import { Cart } from './components/cart/cart';

export const routes: Routes = [
  // أول ما المشروع يفتح (مسار فارغ)، يتم تحويله أوتوماتيك إلى الـ home
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  
  // مسارات الصفحات
  { path: 'home', component: Home },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: Register },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'privacy', component: Privacy },
  { path: 'cart', component: Cart },
  
  // (اختياري) لو كتب أي مسار مش موجود، يرجعه للـ home أو الـ login
  { path: '**', redirectTo: 'home' }
];