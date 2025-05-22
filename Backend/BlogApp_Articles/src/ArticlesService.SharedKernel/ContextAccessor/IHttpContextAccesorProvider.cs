namespace ArticlesService.SharedKernel.ContextAccessor;

public interface IHttpContextAccesorProvider
{
    string GetAccessToken();
    long GetUserId();
    string GetUserRole();
}
