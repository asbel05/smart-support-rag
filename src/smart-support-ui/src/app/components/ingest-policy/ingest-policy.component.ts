import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  POLICY_CATEGORIES,
  PolicyCategory
} from '../../models/support.models';
import { SupportApiService } from '../../services/support-api.service';

@Component({
  selector: 'app-ingest-policy',
  imports: [FormsModule],
  templateUrl: './ingest-policy.component.html',
  styleUrl: './ingest-policy.component.css'
})
export class IngestPolicyComponent {
  private readonly api = inject(SupportApiService);

  readonly categories = POLICY_CATEGORIES;
  readonly isLoading = signal<boolean>(false);
  readonly statusMessage = signal<string>('');
  readonly errorMessage = signal<string>('');

  title = '';
  category: PolicyCategory = 'Devoluciones';
  content = '';

  onSubmit(): void {
    const trimmedTitle = this.title.trim();
    const trimmedContent = this.content.trim();
    if (!trimmedTitle || !trimmedContent) {
      return;
    }

    this.statusMessage.set('');
    this.errorMessage.set('');
    this.isLoading.set(true);

    this.api
      .ingestPolicy({
        title: trimmedTitle,
        category: this.category,
        content: trimmedContent
      })
      .subscribe({
        next: (res) => {
          this.isLoading.set(false);
          this.title = '';
          this.content = '';
          this.statusMessage.set(
            `Política "${res.title}" guardada correctamente (${res.chunksCreated} fragmentos).`
          );
        },
        error: () => {
          this.isLoading.set(false);
          this.errorMessage.set('No se pudo guardar la política.');
        }
      });
  }
}

