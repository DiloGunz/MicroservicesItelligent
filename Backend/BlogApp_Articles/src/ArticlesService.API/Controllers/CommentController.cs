using ArticlesService.API.Controllers.Base;
using ArticlesService.Application.Models.Queries.CommentQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesService.API.Controllers;

[Route("api/[controller]")]
public class CommentController : ApiControllerBase
{
    private readonly ISender _mediator;

    public CommentController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("[action]/{articleId}")]
    public async Task<IActionResult> GetByArticleId(long articleId)
    {
        var response = await _mediator.Send(new GetCommentByArticleIdQuery() { ArticuloId = articleId });
        return response.Match(x => Ok(x), e => Problem(e));
    }
}