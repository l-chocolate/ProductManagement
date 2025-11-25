import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Product, ProductService } from '../../../products/services/product.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.html',
  styleUrls: ['./product-list.scss']
})
export class ProductList implements OnInit {

  products: Product[] = [];

  constructor(private service: ProductService) {}

  ngOnInit(): void {
    this.service.getAll().subscribe({
      next: (data) => this.products = data,
      error: (err) => console.error(err)
    });
  }
}
