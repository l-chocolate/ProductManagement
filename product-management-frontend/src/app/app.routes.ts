import { Routes } from '@angular/router';
import { ProductList } from './products/pages/product-list/product-list';
import { ProductForm } from './products/pages/product-form/product-form';

export const routes: Routes = [
  { path: 'products', component: ProductList },
  { path: 'products/create', component: ProductForm },
  { path: '', redirectTo: 'products', pathMatch: 'full' }
];
