using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Queries.ArticleQueries;
using BlogApp.Proxy.ArticleService.Proxies;
using BlogApp.Shared.Collection;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.FrontEnd.Pages.Articles;

public class IndexModel : PageModel
{
    private readonly IArticleProxy _articleProxy;

    public IndexModel(IArticleProxy articleProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
    }

    public DataCollection<ArticleListDto> ArticleList { get; set; }

    public async Task OnGet()
    {
        ArticleList = await _articleProxy.GetPagedAsync(new GetPagedArticleQuery() { Page = 1, Take = 50});
    }
}