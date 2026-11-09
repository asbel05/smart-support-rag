import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import {
  POLICY_CATEGORIES,
  PagedPoliciesResponse,
  PolicyCategory
} from '../../models/support.models';
import { SupportApiService } from '../../services/support-api.service';

@Component({
  selector: 'app-policies-catalog',
  imports: [FormsModule, DatePipe],
  templateUrl: './policies-catalog.component.html',
  styleUrl: './policies-catalog.component.css'
})
export class PoliciesCatalogComponent implements OnInit, OnDestroy {
  private readonly api = inject(SupportApiService);
  private activeSubscription: Subscription | null = null;

  readonly categories = POLICY_CATEGORIES;
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string>('');
  readonly pagedData = signal<PagedPoliciesResponse | null>(null);
  readonly expandedPolicyId = signal<string | null>(null);

  selectedCategory: PolicyCategory | '' = '';
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadPolicies();
  }

  ngOnDestroy(): void {
    this.cancelPendingRequest();
  }

  toggleExpand(id: string): void {
    if (this.expandedPolicyId() === id) {
      this.expandedPolicyId.set(null);
    } else {
      this.expandedPolicyId.set(id);
    }
  }

  onFilterChange(): void {
    this.pageNumber = 1;
    this.expandedPolicyId.set(null);
    this.loadPolicies();
  }

  goToPage(page: number): void {
    const current = this.pagedData();
    if (!current || page < 1 || page > current.totalPages) {
      return;
    }

    this.pageNumber = page;
    this.expandedPolicyId.set(null);
    this.loadPolicies();
  }

  loadPolicies(): void {
    this.cancelPendingRequest();

    this.errorMessage.set('');
    this.isLoading.set(true);

    const category = this.selectedCategory === '' ? null : this.selectedCategory;

    this.activeSubscription = this.api
      .getPolicies(category, this.pageNumber, this.pageSize)
      .subscribe({
        next: (res) => {
          this.isLoading.set(false);
          this.pagedData.set(res);
        },
        error: (err) => {
          this.isLoading.set(false);
          this.errorMessage.set(
            this.api.extractErrorMessage(err, 'No se pudo cargar el listado de políticas.')
          );
        }
      });
  }

  private cancelPendingRequest(): void {
    if (this.activeSubscription && !this.activeSubscription.closed) {
      this.activeSubscription.unsubscribe();
      this.activeSubscription = null;
    }
  }
}
