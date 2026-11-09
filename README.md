# SmartSupportRag

Asistente RAG para políticas de comercio electrónico. Desde la interfaz puedes registrar políticas, buscar fragmentos y hacer consultas con el contexto recuperado a la vista.

**Stack** · ASP.NET Core (.NET 10), Angular 21, EF Core, PostgreSQL 17 con `pgvector` y Azure OpenAI con `Microsoft.Extensions.AI`.

## Inicio local

Requiere el SDK de .NET 10, Node.js 24, Docker Compose y dos despliegues de Azure OpenAI, uno de chat y otro de embeddings de 1536 dimensiones. Si te falta la CLI de EF Core, instálala con `dotnet tool install --global dotnet-ef --version 10.0.14`.

Desde la raíz del repositorio, inicia PostgreSQL con `docker compose up -d`. Cuando esté listo, configura Azure OpenAI y arranca la API.

```powershell
$env:AzureOpenAI__Endpoint = '<URL del recurso de Azure OpenAI>'
$env:AzureOpenAI__ApiKey = [System.Net.NetworkCredential]::new('', (Read-Host 'Clave de Azure OpenAI' -AsSecureString)).Password
$env:AzureOpenAI__ChatDeploymentName = '<despliegue de chat>'
$env:AzureOpenAI__EmbeddingDeploymentName = '<despliegue de embeddings>'

Set-Location src/SmartSupportRag.Api
dotnet ef database update
dotnet run --launch-profile http
```

En otra terminal, desde la raíz del repositorio, inicia Angular.

```powershell
Set-Location src/smart-support-ui
npm ci
npm start
```

Abre `http://localhost:4200` y selecciona **Cargar políticas base** para empezar a consultar.

## API

La API se sirve en `http://localhost:5296`. En desarrollo, el documento OpenAPI está en `/openapi/v1.json`.

[Peticiones HTTP](src/SmartSupportRag.Api/SmartSupportRag.Api.http) · [Diseño del sistema](docs/SDD.md)
