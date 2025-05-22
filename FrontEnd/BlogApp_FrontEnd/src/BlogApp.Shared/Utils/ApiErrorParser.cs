using System.Text.Json;

namespace BlogApp.Shared.Utils;

public static class ApiErrorParser
{
    public static async Task<List<string>> GetErrorMessages(this HttpResponseMessage response)
    {
        var errores = new List<string>
        {
            //$"{(int)response.StatusCode} {response.ReasonPhrase}"
        };

        if (response.Content == null)
            return errores;

        var contenido = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(contenido);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out JsonElement erroresElemento) && erroresElemento.ValueKind == JsonValueKind.Object)
            {
                foreach (var campo in erroresElemento.EnumerateObject())
                {
                    foreach (var mensaje in campo.Value.EnumerateArray())
                    {
                        errores.Add($"{mensaje.GetString()}");
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
            errores.Add("Error al analizar la respuesta JSON del servidor.");
        }

        return errores;
    }

}
