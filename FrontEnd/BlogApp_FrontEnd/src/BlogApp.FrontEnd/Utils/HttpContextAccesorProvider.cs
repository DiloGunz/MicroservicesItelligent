using BlogApp.Shared.ContextAccessor;
using System.Security.Claims;

namespace BlogApp.FrontEnd.Utils;

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
        try
        {
            var user = _contextAccessor.HttpContext!.User;

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var identity = user.Identity;

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var claimUserData = ((ClaimsIdentity)identity).FindFirst("access_token");

            if (claimUserData == null)
            {
                throw new ArgumentNullException(nameof(claimUserData));
            }

            var accessToken = claimUserData.Value;

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            return accessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"ERROR GET TOKEN: {ex.Message}");
            return "";
        }
    }

    public CurrentUserDto GetCurrentUser()
    {
        try
        {
            var user = _contextAccessor.HttpContext!.User;

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var identity = user.Identity;

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var claimUserName = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.Name);
            var claimUserId = ((ClaimsIdentity)identity).FindFirst(ClaimTypes.NameIdentifier);

            var result = new CurrentUserDto()
            {
                UserName = claimUserName!.Value,
                Id = Convert.ToInt64(claimUserId!.Value)
            };

            return result;
        }
        catch (Exception)
        {
            return new CurrentUserDto();
        }
    }
}