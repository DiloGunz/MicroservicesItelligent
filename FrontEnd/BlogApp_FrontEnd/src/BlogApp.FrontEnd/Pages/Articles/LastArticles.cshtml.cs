using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Queries.ArticleQueries;
using AuthService.Application.Models.AppUserQueries;
using BlogApp.Proxy.ArticleService.Proxies;
using BlogApp.Proxy.AuthService.Proxies;
using BlogApp.Shared.Collection;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.FrontEnd.Pages.Articles;

public class LastArticlesModel : PageModel
{
    private readonly IArticleProxy _articleProxy;
    private readonly IUserProxy _userProxy;

    public LastArticlesModel(IArticleProxy articleProxy, IUserProxy userProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
        _userProxy = userProxy ?? throw new ArgumentNullException(nameof(userProxy));
    }

    public DataCollection<ArticleDetailsDto> ArticlesCollection { get; private set; }

    public async Task OnGet(int id = 1)
    {
        var query = new GetPagedLastArticleQuery()
        {
            Page = id,
            Take = 5
        };
        ArticlesCollection = await _articleProxy.GetPagedLastAsync(query);
        await SetUserNameAsync(ArticlesCollection.Items);
    }

    private async Task SetUserNameAsync(IEnumerable<ArticleDetailsDto> articles)
    {
        var ids = articles.Select(x => x.CreatedBy).ToList();

        var users = await _userProxy.GetByIdsAsync(new GetAppUserByIdsQuery()
        {
            Ids = ids
        });

        foreach (var item in articles)
        {
            item.CreatedByName = users.FirstOrDefault(x => x.Id == item.CreatedBy)!.Username;
        }
    }
}