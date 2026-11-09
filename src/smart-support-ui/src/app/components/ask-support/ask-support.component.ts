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
  readonly copied = signal<boolean>(false);

  question = '';
  selectedCategory: PolicyCategory | '' = '';
  topK = 3;

  onSubmit(): void {
    const trimmed = this.question.trim();
    if (!trimmed) {
      return;
    }

    this.errorMessage.set('');
    this.copied.set(false);
    this.askResult.set(null);
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
        error: (err) => {
          this.isLoading.set(false);
          this.askResult.set(null);
          this.errorMessage.set(
            this.api.extractErrorMessage(err, 'No se pudo obtener respuesta del asistente.')
          );
        }
      });
  }

  copyAnswer(text: string): void {
    if (!text) {
      return;
    }

    navigator.clipboard.writeText(text).then(() => {
      this.copied.set(true);
      setTimeout(() => {
        this.copied.set(false);
      }, 2000);
    });
  }
}
