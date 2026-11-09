import { DecimalPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  AskSupportResponse,
  POLICY_CATEGORIES,
  PolicyCategory
} from '../../models/support.models';
import { SupportApiService } from '../../services/support-api.service';

@Component({
  selector: 'app-ask-support',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './ask-support.component.html',
  styleUrl: './ask-support.component.css'
})
export class AskSupportComponent {
  private readonly api = inject(SupportApiService);

  readonly categories = POLICY_CATEGORIES;
  readonly isLoading = signal<boolean>(false);
  readonly errorMessage = signal<string>('');
  readonly askResult = signal<AskSupportResponse | null>(null);

  question = '';
  selectedCategory: PolicyCategory | '' = '';
  topK = 3;

  onSubmit(): void {
    const trimmed = this.question.trim();
    if (!trimmed) {
      return;
    }

    this.errorMessage.set('');
    this.isLoading.set(true);

    this.api
      .askSupport({
        question: trimmed,
        category: this.selectedCategory === '' ? null : this.selectedCategory,
        topK: this.topK
      })
      .subscribe({
        next: (res) => {
          this.isLoading.set(false);
          this.askResult.set(res);
        },
        error: () => {
          this.isLoading.set(false);
          this.errorMessage.set('No se pudo obtener respuesta del asistente.');
        }
      });
  }
}

