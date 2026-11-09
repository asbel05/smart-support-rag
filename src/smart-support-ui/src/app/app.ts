import { Component, inject, signal } from '@angular/core';
import { AskSupportComponent } from './components/ask-support/ask-support.component';
import { IngestPolicyComponent } from './components/ingest-policy/ingest-policy.component';
import { PoliciesCatalogComponent } from './components/policies-catalog/policies-catalog.component';
import { SearchPoliciesComponent } from './components/search-policies/search-policies.component';
import { SupportApiService } from './services/support-api.service';

type ActiveTab = 'ask' | 'search' | 'ingest';

@Component({
  selector: 'app-root',
  imports: [
    AskSupportComponent,
    PoliciesCatalogComponent,
    SearchPoliciesComponent,
    IngestPolicyComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly api = inject(SupportApiService);

  readonly activeTab = signal<ActiveTab>('ask');
  readonly isSeeding = signal<boolean>(false);
  readonly isCatalogModalOpen = signal<boolean>(false);
  readonly statusMessage = signal<string>('');
  readonly errorMessage = signal<string>('');

  selectTab(tab: ActiveTab): void {
    this.activeTab.set(tab);
    this.statusMessage.set('');
    this.errorMessage.set('');
  }

  openCatalogModal(): void {
    this.isCatalogModalOpen.set(true);
  }

  closeCatalogModal(): void {
    this.isCatalogModalOpen.set(false);
  }

  onSeedPolicies(): void {
    this.statusMessage.set('');
    this.errorMessage.set('');
    this.isSeeding.set(true);

    this.api.seedPolicies().subscribe({
      next: (res) => {
        this.isSeeding.set(false);
        this.statusMessage.set(
          `${res.message} (${res.documentsCreated} documentos, ${res.totalChunksCreated} fragmentos).`
        );
      },
      error: (err) => {
        this.isSeeding.set(false);
        this.errorMessage.set(
          this.api.extractErrorMessage(
            err,
            'No se pudo conectar con la API para cargar las políticas base.'
          )
        );
      }
    });
  }
}
