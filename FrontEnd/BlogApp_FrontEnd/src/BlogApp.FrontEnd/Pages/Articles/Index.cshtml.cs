using ArticleService.Application.Models.Commands.ArticleCommands;
using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Queries.ArticleQueries;
using AuthService.Application.Models.AppUserQueries;
using BlogApp.Proxy.ArticleService.Proxies;
using BlogApp.Proxy.AuthService.Proxies;
using BlogApp.Shared.Collection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogApp.FrontEnd.Pages.Articles;

public class IndexModel : PageModel
{
    private readonly IArticleProxy _articleProxy;
    private readonly IUserProxy _userProxy;

    public IndexModel(IArticleProxy articleProxy, IUserProxy userProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
        _userProxy = userProxy ?? throw new ArgumentNullException(nameof(userProxy));
    }

    public DataCollection<ArticleListDto> ArticleList { get; private set; }

    public async Task OnGet()
    {
        ArticleList = await _articleProxy.GetPagedAsync(new GetPagedArticleQuery());
        await SetUserNameAsync(ArticleList.Items);
    }

    public async Task<IActionResult> OnPostDeleteAsync(long id)
    {
        var cmd = new DeleteArticleCommand
        {
            Id = id
        };

        var response = await _articleProxy.DeleteAsync(cmd);
        return new JsonResult(response);
    }

    public async Task<IActionResult> OnGetSearchAsync(string criteria, int p = 1)
    {
        if (p < 0)
        {
            p = 1;
        }
        var query = new GetPagedArticleQuery()
        { Criteria = criteria, Page = p};
        var data = await _articleProxy.GetPagedAsync(query);
        await SetUserNameAsync(data.Items);
        return new PartialViewResult()
        {
            ViewName = "_ArticlesList",
            ViewData = new ViewDataDictionary<DataCollection<ArticleListDto>>(ViewData, data)
        };
    }

    private async Task SetUserNameAsync(IEnumerable<ArticleListDto> articles)
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