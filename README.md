# SmartSupportRag

Asistente de soporte al cliente para plataformas de comercio electrónico basado en **RAG (Retrieval-Augmented Generation)** y búsqueda vectorial en **PostgreSQL con `pgvector`**.

Clasifica consultas en 5 políticas base (`Devoluciones`, `Garantias`, `Envios`, `Terminos`, `Seguridad`), recupera fragmentos mediante distancia coseno y genera respuestas fundamentadas con un LLM.

---

## Stack Tecnológico

- **Backend:** .NET 10 (ASP.NET Core Web API con Controllers clásicos)
- **Datos:** EF Core 10 + Npgsql + `Pgvector.EntityFrameworkCore`
- **Base de Datos:** PostgreSQL 17 + `pgvector` (Docker)
- **IA:** `Microsoft.Extensions.AI`
- **Frontend:** Angular (Fase Final)

---

## Documentación Técnica

Para arquitectura, modelo de datos y contratos de endpoints:

👉 **[docs/SDD.md](./docs/SDD.md)**

---

## Convención de Commits

- `docs:` Documentación.
- `chore:` Infraestructura, Docker, dependencias.
- `feat:` Funcionalidades, endpoints, modelos o servicios.
- `fix:` Corrección de errores.
