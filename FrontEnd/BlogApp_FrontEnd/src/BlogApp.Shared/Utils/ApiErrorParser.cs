using System.Net;
using System.Text.Json;

namespace BlogApp.Shared.Utils;

public static class ApiErrorParser
{
    public static async Task<List<string>> GetErrorMessages(this HttpResponseMessage response)
    {
        var errores = new List<string>();

        if (response.Content == null)
        {
            errores.Add($"{(int)response.StatusCode} {response.ReasonPhrase}");
            return errores;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized: // 401
                errores.Add("No estás autenticado. Inicia sesión e intenta de nuevo.");
                return errores;

            case HttpStatusCode.Forbidden: // 403
                errores.Add("No tienes permisos para realizar esta acción (403 - Forbidden).");
                return errores;

            case HttpStatusCode.NotFound: // 404
                errores.Add("Recurso no encontrado (404).");
                return errores;

            case HttpStatusCode.InternalServerError: // 500
                errores.Add("Error interno del servidor. Intenta más tarde.");
                break;
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            errores.Add($"{(int)response.StatusCode} {response.ReasonPhrase}");
            return errores;
        }

        try
        {
            if (!contenido.TrimStart().StartsWith("{") && !contenido.TrimStart().StartsWith("["))
            {
                errores.Add(contenido);
                return errores;
            }

            using var doc = JsonDocument.Parse(contenido);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out JsonElement erroresElemento) && erroresElemento.ValueKind == JsonValueKind.Object)
            {
                foreach (var campo in erroresElemento.EnumerateObject())
                {
                    foreach (var mensaje in campo.Value.EnumerateArray())
                    {
                        errores.Add(mensaje.GetString());
                    }
                }
            }
            else if (root.TryGetProperty("detail", out JsonElement detalle))
            {
                errores.Add(detalle.GetString());
            }
            else if (root.TryGetProperty("title", out JsonElement titulo))
            {
                errores.Add(titulo.GetString());
            }
            else
            {
                errores.Add(contenido);
            }
        }
        catch (JsonException)
        {
            errores.Add($"Error al analizar JSON. Contenido recibido: {contenido}");
        }

        return errores;
    }

}
