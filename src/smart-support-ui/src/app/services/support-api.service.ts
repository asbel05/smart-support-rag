import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  AskSupportRequest,
  AskSupportResponse,
  IngestPolicyRequest,
  IngestPolicyResponse,
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
}

