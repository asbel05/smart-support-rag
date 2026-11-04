# Reglas para Agentes de IA (AGENTS.md)

Directivas técnicas obligatorias para cualquier asistente de IA en este proyecto:

1. **Controladores Clásicos (.NET 10):**
   - Usar siempre `ControllerBase`, `[ApiController]` y atributos de ruta.
   - Prohibido el uso de Minimal APIs (`app.MapGet`, `app.MapPost`).

2. **Fuente de la Verdad:**
   - Toda entidad, endpoint, lógica de chunking o consulta vectorial debe respetar la especificación en `docs/SDD.md`.

3. **Flujo de Trabajo Incremental:**
   - Avanzar una sola tarea a la vez. No generar múltiples capas de código de golpe.
   - Al terminar cada tarea, detenerse para verificación del desarrollador antes del commit.

4. **Aprendizaje Activo de Infraestructura:**
   - La configuración de Docker (`docker-compose.yml`) y comandos de Git son realizados manualmente por el desarrollador para fines de aprendizaje. La IA guía, explica y revisa, pero no los asume automáticamente.

5. **Frontend:**
   - Se implementará en Angular únicamente tras validar y probar todos los endpoints del backend.
