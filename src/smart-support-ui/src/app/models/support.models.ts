export type PolicyCategory =
  | 'Devoluciones'
  | 'Garantias'
  | 'Envios'
  | 'Terminos'
  | 'Seguridad';

export const POLICY_CATEGORIES: readonly PolicyCategory[] = [
  'Devoluciones',
  'Garantias',
  'Envios',
  'Terminos',
  'Seguridad'
];

export interface AskSupportRequest {
  question: string;
  category?: PolicyCategory | null;
  topK: number;
}

export interface SupportSourceDto {
  chunkId: string;
  documentId: string;
  documentTitle: string;
  category: PolicyCategory;
  chunkIndex: number;
  content: string;
  similarityScore: number;
}

export interface AskSupportResponse {
  question: string;
  categoryFilter?: PolicyCategory | null;
  answer: string;
  sources: SupportSourceDto[];
}

export interface SearchPoliciesRequest {
  query: string;
  category?: PolicyCategory | null;
  topK: number;
}

export interface PolicyChunkMatchResponse {
  chunkId: string;
  documentId: string;
  documentTitle: string;
  category: PolicyCategory;
  chunkIndex: number;
  content: string;
  cosineDistance: number;
  similarityScore: number;
}

export interface SearchPoliciesResponse {
  query: string;
  categoryFilter?: PolicyCategory | null;
  totalMatches: number;
  matches: PolicyChunkMatchResponse[];
}

export interface IngestPolicyRequest {
  title: string;
  category: PolicyCategory;
  content: string;
}

export interface IngestPolicyResponse {
  documentId: string;
  title: string;
  category: PolicyCategory;
  chunksCreated: number;
}

export interface SeedPoliciesResponse {
  documentsCreated: number;
  totalChunksCreated: number;
  message: string;
}

export interface PolicyChunkItemResponse {
  id: string;
  chunkIndex: number;
  content: string;
}

export interface PolicySummaryResponse {
  id: string;
  title: string;
  category: PolicyCategory;
  createdAt: string;
  chunksCount: number;
  chunks: PolicyChunkItemResponse[];
}

export interface PagedPoliciesResponse {
  items: PolicySummaryResponse[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
