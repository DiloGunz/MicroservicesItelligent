namespace BlogApp.Shared.ContextAccessor;

public interface IHttpContextAccesorProvider
{
    string GetAccessToken();
    string GetCurrentUserAsJson();
    CurrentUserDto GetCurrentUser();
}
