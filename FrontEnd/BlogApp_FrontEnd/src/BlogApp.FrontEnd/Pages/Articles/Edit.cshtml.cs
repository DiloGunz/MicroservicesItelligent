using ArticleService.Application.Models.Commands.ArticleCommands;
using ArticleService.Application.Models.Dtos.ArticlesDtos;
using BlogApp.Proxy.ArticleService.Proxies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.FrontEnd.Pages.Articles;

public class EditModel : PageModel
{
    private readonly IArticleProxy _articleProxy;

    public EditModel(IArticleProxy articleProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
    }

    public ArticleDetailsDto ArticleDto { get; private set; }

    public async Task<IActionResult> OnGet(long id)
    {
        var article = await _articleProxy.GetByIdAsync(id);
        if (article == null)
        {
            return RedirectToPage("/Articles/Index");
        }

        ArticleDto = article;
        return Page();
    }

    public async Task<IActionResult> OnPost([FromBody] EditArticleCommand command)
    {
        var response = await _articleProxy.EditAsync(command);
        if (response.IsSuccess)
        {
            response.Url = "/Articles/index";
        }
        return new JsonResult(response);
    }
}