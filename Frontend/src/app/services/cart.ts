import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from '../components/home/home';

export interface CartItem {
  product: Product;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private storageKey = 'novacart_items';
  private items: CartItem[] = [];

  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  constructor() {
    this.loadFromStorage(); 
  }

  private loadFromStorage() {
    const saved = localStorage.getItem(this.storageKey);
    if (saved) {
      this.items = JSON.parse(saved);
      this.updateCount();
    }
  }

  private saveToStorage() {
    localStorage.setItem(this.storageKey, JSON.stringify(this.items));
    this.updateCount();
  }

  private updateCount() {
    const total = this.items.reduce((sum, item) => sum + item.quantity, 0);
    this.cartCountSubject.next(total);
  }

  getItems(): CartItem[] {
    return this.items;
  }

  addToCart(product: Product) {
    const existing = this.items.find(i => i.product.id === product.id);
    if (existing) {
      existing.quantity++;
    } else {
      this.items.push({ product, quantity: 1 });
    }
    this.saveToStorage();
  }

  updateQuantity(productId: number, change: number) {
    const item = this.items.find(i => i.product.id === productId);
    if (item) {
      item.quantity += change;
      if (item.quantity <= 0) {
        this.removeItem(productId);
      } else {
        this.saveToStorage();
      }
    }
  }

  removeItem(productId: number) {
    this.items = this.items.filter(i => i.product.id !== productId);
    this.saveToStorage();
  }

  getTotalPrice(): number {
    return this.items.reduce((sum, item) => sum + (item.product.price * item.quantity), 0);
  }

  clearCart() {
    this.items = [];
    this.saveToStorage();
  }
}