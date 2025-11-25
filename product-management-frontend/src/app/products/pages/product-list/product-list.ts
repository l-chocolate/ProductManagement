import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Observable, map } from 'rxjs';
import { Product, ProductService } from '../../../products/services/product.service';

type ProductListViewModel = {
  products: Product[];
  total: number;
  averageCost: number;
  highestCost: number;
  topCategory: string;
};

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.html',
  styleUrls: ['./product-list.scss']
})
export class ProductList {
  vm$!: Observable<ProductListViewModel>;

  constructor(private service: ProductService) {
    this.vm$ = this.service.getAll().pipe(
      map(products => this.buildViewModel(products))
    );
  }

  private buildViewModel(products: Product[]): ProductListViewModel {
    const total = products.length;
    const costs = products.map(p => Number(p.unitCost ?? 0));
    const totalCost = costs.reduce((sum, cost) => sum + cost, 0);
    const highestCost = costs.reduce((max, cost) => Math.max(max, cost), 0);
    const categoryCounter = products.reduce<Record<string, number>>((acc, product) => {
      const category = product.categoryName?.trim() || 'Uncategorized';
      acc[category] = (acc[category] ?? 0) + 1;
      return acc;
    }, {});

    const topCategory = total
      ? Object.entries(categoryCounter).sort((a, b) => b[1] - a[1])[0][0]
      : '—';

    return {
      products,
      total,
      averageCost: total ? totalCost / total : 0,
      highestCost,
      topCategory
    };
  }
}
