namespace BlogApp.Shared.ContextAccessor;

public interface IHttpContextAccesorProvider
{
    string GetAccessToken();
    CurrentUserDto GetCurrentUser();
}
