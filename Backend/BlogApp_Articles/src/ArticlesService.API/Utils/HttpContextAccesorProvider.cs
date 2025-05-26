using ArticlesService.SharedKernel.ContextAccessor;
using System.Security.Authentication;

namespace ArticlesService.API.Utils;

public class HttpContextAccesorProvider : IHttpContextAccesorProvider
{
    private readonly ILogger<HttpContextAccesorProvider> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextAccesorProvider(ILogger<HttpContextAccesorProvider> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public string GetAccessToken()
    {
        var token = _contextAccessor.HttpContext?.Items["AccessToken"]?.ToString();
        return token ?? "";
    }

    public long GetUserId()
    {
        try
        {
            var userId = _contextAccessor.HttpContext?.Items["UserId"]?.ToString();
            var longId = long.Parse(userId ?? "0");
            if (longId <= 0)
            {
                throw new AuthenticationException("Error de usuario");
            }
            return longId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return 0;
        }
    }

    public string GetUserRole()
    {
        try
        {
            var userRole = _contextAccessor.HttpContext?.Items["UserRole"]?.ToString();
            if (string.IsNullOrWhiteSpace(userRole))
            {
                throw new AuthenticationException("Error de rol");
            }
            return userRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return "";
        }
    }
}