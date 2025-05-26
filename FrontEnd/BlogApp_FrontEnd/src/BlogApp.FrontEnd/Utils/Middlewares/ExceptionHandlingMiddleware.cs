using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BlogApp.FrontEnd.Utils.Middlewares;

public class GloblalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GloblalExceptionHandlingMiddleware> _logger;

    public GloblalExceptionHandlingMiddleware(ILogger<GloblalExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            if (!context.Response.HasStarted)
            {
                context.Response.Redirect("/Error");
            }
            else
            {
                _logger.LogWarning("No se pudo redirigir a /Error porque la respuesta ya había comenzado.");
                throw; 
            }
        }
    }
}
