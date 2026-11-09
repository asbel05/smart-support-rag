import { DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  POLICY_CATEGORIES,
  PolicyCategory,
  SearchPoliciesResponse
} from '../../models/support.models';
import { SupportApiService } from '../../services/support-api.service';

@Component({
  selector: 'app-search-policies',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './search-policies.component.html',
  styleUrl: './search-policies.component.css'
})
export class SearchPoliciesComponent {
  private readonly api = inject(SupportApiService);

  readonly categories = POLICY_CATEGORIES;
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string>('');
  readonly searchResult = signal<SearchPoliciesResponse | null>(null);

  searchQuery = '';
  selectedCategory: PolicyCategory | '' = '';
  topK = 3;

  onSubmit(): void {
    const trimmed = this.searchQuery.trim();
    if (!trimmed) {
      return;
    }

    this.errorMessage.set('');
    this.searchResult.set(null);
    this.isLoading.set(true);

    this.api
      .searchPolicies({
        query: trimmed,
        category: this.selectedCategory === '' ? null : this.selectedCategory,
        topK: this.topK
      })
      .subscribe({
        next: (res) => {
          this.isLoading.set(false);
          this.searchResult.set(res);
        },
        error: (err) => {
          this.isLoading.set(false);
          this.searchResult.set(null);
          this.errorMessage.set(
            this.api.extractErrorMessage(err, 'No se pudo realizar la búsqueda de fragmentos.')
          );
        }
      });
  }
}

