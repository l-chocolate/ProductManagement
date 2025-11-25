import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ProductService } from '../../../products/services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.html',
  styleUrls: ['./product-form.scss']
})
export class ProductForm {

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private service: ProductService,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: [''],
      categoryName: [''],
      unitCost: [0]
    });
  }

  submit() {
    this.service.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/products']),
      error: err => console.error(err)
    });
  }
}
