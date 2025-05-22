using ArticlesService.SharedKernel.ContextAccessor;

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
            return long.Parse(userId ?? "0");
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
            return userRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return "";
        }
    }
}