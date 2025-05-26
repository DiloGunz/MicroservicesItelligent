using ArticleService.Application.Models.Commands.ArticleCommands;
using BlogApp.Proxy.ArticleService.Proxies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.FrontEnd.Pages.Articles;

public class CreateModel : PageModel
{
    private readonly IArticleProxy _articleProxy;

    public CreateModel(IArticleProxy articleProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost([FromBody] CreateArticleCommand command)
    {
        var response = await _articleProxy.CreateAsync(command);
        if (response.IsSuccess)
        {
            response.Url = "/Articles/index";
        }
        return new JsonResult(response);
    }
}