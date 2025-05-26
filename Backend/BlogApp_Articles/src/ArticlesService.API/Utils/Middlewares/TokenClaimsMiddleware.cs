using System.Security.Claims;

namespace ArticlesService.API.Utils.Middlewares;

public class TokenClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public TokenClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user.FindFirst("role")?.Value;

            var listClaims = user.Claims.ToList();

            context.Items["UserId"] = userId;
            context.Items["UserRole"] = role;
        }

        await _next(context);
    }
}