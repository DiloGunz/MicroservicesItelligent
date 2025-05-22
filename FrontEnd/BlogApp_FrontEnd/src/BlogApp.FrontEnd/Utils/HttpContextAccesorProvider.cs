using BlogApp.Shared.ContextAccessor;
using System.Security.Claims;
using System.Text.Json;

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
            var jUser = GetCurrentUserAsJson();

            var userDto = JsonSerializer.Deserialize<CurrentUserDto>(jUser);

            if (userDto is null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            return userDto;
        }
        catch (Exception)
        {
            return new CurrentUserDto();
        }
    }

    public string GetCurrentUserAsJson()
    {
        string jUser = "{}";
        try
        {
            if (_contextAccessor.HttpContext != null)
            {
                var user = _contextAccessor.HttpContext.User;

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var identity = user.Identity;

                if (identity == null)
                {
                    throw new ArgumentNullException(nameof(identity));
                }

                //if (!identity.IsAuthenticated)
                //{
                //    throw new InvalidOperationException("ERROR DE LOGUEO");
                //}


                var claimUserData = ((ClaimsIdentity)identity).FindFirst("user_json");

                if (claimUserData == null)
                {
                    throw new ArgumentNullException(nameof(claimUserData));
                }

                jUser = claimUserData.Value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"ERROR GET USER AS JSON: {ex.Message}");
            return "{}";
        }

        return jUser;
    }
}