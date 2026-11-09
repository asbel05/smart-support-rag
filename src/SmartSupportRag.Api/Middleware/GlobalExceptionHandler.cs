using System.ClientModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SmartSupportRag.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Excepción no controlada procesando {Path}", httpContext.Request.Path);

        var (statusCode, title, detail) = exception switch
        {
            ArgumentException argEx => (
                StatusCodes.Status400BadRequest,
                "Solicitud inválida",
                argEx.Message),

            ClientResultException => (
                StatusCodes.Status502BadGateway,
                "Error en el servicio de Inteligencia Artificial",
                "No se pudo completar la operación con el proveedor de modelos de IA."),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Error interno del servidor",
                "Ocurrió un error inesperado al procesar la solicitud.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

