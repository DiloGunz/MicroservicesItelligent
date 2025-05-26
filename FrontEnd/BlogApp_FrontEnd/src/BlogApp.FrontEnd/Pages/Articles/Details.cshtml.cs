using ArticleService.Application.Models.Commands.CommentCommands;
using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Dtos.CommentDtos;
using AuthService.Application.Models.AppUserQueries;
using BlogApp.Proxy.ArticleService.Proxies;
using BlogApp.Proxy.AuthService.Proxies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogApp.FrontEnd.Pages.Articles;

public class DetailsModel : PageModel
{
    private readonly IArticleProxy _articleProxy;
    private readonly ICommentProxy _commentProxy;
    private readonly IUserProxy _userProxy;

    public DetailsModel(IArticleProxy articleProxy, ICommentProxy commentProxy, IUserProxy userProxy)
    {
        _articleProxy = articleProxy ?? throw new ArgumentNullException(nameof(articleProxy));
        _commentProxy = commentProxy ?? throw new ArgumentNullException(nameof(commentProxy));
        _userProxy = userProxy ?? throw new ArgumentNullException(nameof(userProxy));
    }

    public ArticleDetailsDto ArticleDto { get; private set; }
    public List<CommentDto> Comments { get; private set; }

    public async Task<IActionResult> OnGet(long id)
    {
        var article = await _articleProxy.GetByIdAsync(id);
        if (article == null)
        {
            return RedirectToPage("/Articles/Index");
        }

        Comments = await _commentProxy.GetByArticleAsync(id);
        ArticleDto = article;
        await SetUserNameAsync(Comments);
        return Page();
    }

    public async Task<IActionResult> OnPostCreateCommentAsync([FromBody] CreateCommentCommand command)
    {
        var response = await _commentProxy.CreateAsync(command);
        return new JsonResult(response);
    }

    public async Task<IActionResult> OnGetCommentsByArticleAsync(long id)
    {
        var response = await _commentProxy.GetByArticleAsync(id);
        ArticleDto = await _articleProxy.GetByIdAsync(id);
        await SetUserNameAsync(response);
        return new PartialViewResult()
        {
            ViewName = "_ListComments",
            ViewData = new ViewDataDictionary<List<CommentDto>>(ViewData, response)
        };
    }

    public async Task<IActionResult> OnPostSaveCommentAsync([FromBody] CreateCommentCommand command)
    {
        var response = await _commentProxy.CreateAsync(command);
        return new JsonResult(response);
    }

    private async Task SetUserNameAsync(List<CommentDto> comments)
    {
        var ids = comments.Select(x => x.CreatedBy).ToList();
        if (ArticleDto != null)
        {
            ids.Add(ArticleDto.CreatedBy);
        }

        var users = await _userProxy.GetByIdsAsync(new GetAppUserByIdsQuery()
        {
            Ids = ids
        });

        foreach (var item in comments)
        {
            item.CreatedByName = users.FirstOrDefault(x => x.Id == item.CreatedBy)!.Username;
        }

        if (ArticleDto != null)
        {
            ArticleDto.CreatedByName = users.FirstOrDefault(x => x.Id == ArticleDto.CreatedBy)!.Username;
        }
    }
}