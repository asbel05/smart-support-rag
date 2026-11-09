import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  AskSupportRequest,
  AskSupportResponse,
  IngestPolicyRequest,
  IngestPolicyResponse,
  PagedPoliciesResponse,
  PolicyCategory,
  SearchPoliciesRequest,
  SearchPoliciesResponse,
  SeedPoliciesResponse
} from '../models/support.models';

@Injectable({
  providedIn: 'root'
})
export class SupportApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5296/api';

  getPolicies(
    category?: PolicyCategory | null,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PagedPoliciesResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (category) {
      params = params.set('category', category);
    }

    return this.http.get<PagedPoliciesResponse>(`${this.baseUrl}/policies`, { params });
  }

  askSupport(request: AskSupportRequest): Observable<AskSupportResponse> {
    return this.http.post<AskSupportResponse>(`${this.baseUrl}/support/ask`, request);
  }

  searchPolicies(request: SearchPoliciesRequest): Observable<SearchPoliciesResponse> {
    return this.http.post<SearchPoliciesResponse>(`${this.baseUrl}/policies/search`, request);
  }

  ingestPolicy(request: IngestPolicyRequest): Observable<IngestPolicyResponse> {
    return this.http.post<IngestPolicyResponse>(`${this.baseUrl}/policies/ingest`, request);
  }

  seedPolicies(): Observable<SeedPoliciesResponse> {
    return this.http.post<SeedPoliciesResponse>(`${this.baseUrl}/policies/seed`, {});
  }

  extractErrorMessage(err: unknown, fallback: string): string {
    if (err instanceof HttpErrorResponse) {
      const detail = err.error?.detail || err.error?.title;
      if (typeof detail === 'string' && detail.trim().length > 0) {
        return detail;
      }
      if (err.status === 429) {
        return 'Se alcanzó el límite de 6 peticiones por minuto. Intenta nuevamente en unos segundos.';
      }
    }
    return fallback;
  }
}
