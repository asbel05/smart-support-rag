# SmartSupportRag

Asistente de soporte al cliente para plataformas de comercio electrónico basado en **RAG (Retrieval-Augmented Generation)** y búsqueda vectorial en **PostgreSQL con `pgvector`**.

Permite ingestar, fragmentar y consultar políticas oficiales en 5 categorías base (`Devoluciones`, `Garantias`, `Envios`, `Terminos`, `Seguridad`), recuperando fragmentos relevantes por similitud de distancia coseno (`<=>` sobre índice HNSW) y generando respuestas precisas con citas de fuentes oficiales mediante un LLM (`gpt-6-luna`).

---

## Capacidades del Sistema

- **Ingesta y Fragmentación:** Fragmentación semántica con solapamiento (`overlap`) y cálculo de embeddings (1536 dimensiones) mediante `text-embedding-3-small`.
- **Búsqueda Vectorial:** Búsqueda por distancia coseno con opción de filtrado explícito por categoría de soporte o búsqueda transversal en toda la base de conocimiento.
- **Asistente RAG Grounded:** Generación de respuestas asistenciales con anclaje estricto en los fragmentos de políticas recuperados y citación explícita de documentos oficiales para mitigar alucinaciones.

---

## Stack Tecnológico

- **Backend:** .NET 10 (ASP.NET Core Web API con Controllers clásicos)
- **Datos:** EF Core 10 + Npgsql + `Pgvector.EntityFrameworkCore`
- **Base de Datos:** PostgreSQL 17 + `pgvector` en Docker (puerto host `5434`)
- **IA:** `Microsoft.Extensions.AI` (`Azure.AI.OpenAI` v2)
- **Frontend:** Angular (Fase Final)

---

## Documentación Técnica

Para arquitectura detallada, estructura en capas, modelo de datos y contratos de endpoints:

👉 **[docs/SDD.md](./docs/SDD.md)**

---

## Convención de Commits

- `docs:` Documentación.
- `chore:` Infraestructura, Docker, dependencias.
- `feat:` Funcionalidades, endpoints, modelos o servicios.
- `fix:` Corrección de errores.
