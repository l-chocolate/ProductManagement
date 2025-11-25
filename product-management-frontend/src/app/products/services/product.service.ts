import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  productId: number;
  name: string;
  categoryName: string;
  unitCost: number;
  createdAt: string;
}

export interface CreateProductRequest {
  name: string;
  categoryName: string;
  unitCost: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'http://localhost:8080/products';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  create(product: CreateProductRequest): Observable<any> {
    return this.http.post(this.apiUrl, product);
  }
}
