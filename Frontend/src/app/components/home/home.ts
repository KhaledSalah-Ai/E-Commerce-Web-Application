import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart';

export interface Product {
  id: number;
  name: string;
  price: number;
  emoji: string;         // يظهر لو مفيش صورة
  img?: string;          // مثال: 'assets/phone.jpg' (اختياري)
  badge?: string;        // مثال: 'الأكثر مبيعاً' (اختياري)
  rating: number;        // من 0 إلى 5
  reviews: number;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, RouterLink, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  cartCount = 0;
  searchTerm: string = '';

  products: Product[] = [
    { id: 1, name: 'هاتف ذكي متطور', price: 699, emoji: '📱', badge: 'الأكثر مبيعاً', rating: 4, reviews: 128 },
    { id: 2, name: 'لابتوب للألعاب والأعمال', price: 1299, emoji: '💻', badge: 'جديد', rating: 5, reviews: 86 },
    { id: 3, name: 'ساعة ذكية رياضية', price: 199, emoji: '⌚', rating: 4, reviews: 214 },
    { id: 4, name: 'سماعات لاسلكية عازلة للضوضاء', price: 149, emoji: '🎧', badge: 'خصم 20%', rating: 4, reviews: 342 },
  ];

  constructor(private router: Router, private cartService: CartService) {}

  ngOnInit() {
    this.cartService.cartCount$.subscribe(count => {
      this.cartCount = count;
    });
  }

  get filteredProducts(): Product[] {
    const query = this.searchTerm.trim().toLowerCase();
    if (!query) {
      return this.products;
    }
    return this.products.filter(p =>
      p.name.toLowerCase().includes(query)
    );
  }

  clearSearch() {
    this.searchTerm = '';
  }

  stars(rating: number): string {
    return '★'.repeat(rating) + '☆'.repeat(5 - rating);
  }

  addToCart(product: Product) {
    this.cartService.addToCart(product);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}