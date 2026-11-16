import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './contact.html',
  styleUrl: './contact.css'
})
export class Contact {
  
  // أضف هذه الدالة لحل المشكلة
  onSubmit() {
    console.log('Form Submitted!');
    alert('تم إرسال رسالتك بنجاح! شكراً لتواصلك معنا 🚀');
  }

}