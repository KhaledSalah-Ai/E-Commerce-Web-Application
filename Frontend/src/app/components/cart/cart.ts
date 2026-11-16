import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CartService, CartItem } from '../../services/cart';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class Cart implements OnInit {
  items: CartItem[] = [];

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.items = this.cartService.getItems();
  }

  increase(productId: number) {
    this.cartService.updateQuantity(productId, 1);
    this.loadCart();
  }

  decrease(productId: number) {
    this.cartService.updateQuantity(productId, -1);
    this.loadCart();
  }

  remove(productId: number) {
    this.cartService.removeItem(productId);
    this.loadCart();
  }

  clear() {
    this.cartService.clearCart();
    this.loadCart();
  }

  get total(): number {
    return this.cartService.getTotalPrice();
  }

  checkout() {
    alert('تم استلام طلبك بنجاح! الإجمالي: $' + this.total);
    this.clear();
  }
}