using ArticlesService.API.Controllers.Base;
using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Application.Models.Commands.CommentCommands;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesService.API.Controllers;

[Route("api/articles")]
public class ArticleController : ApiControllerBase
{
    private readonly ISender _mediator;

    public ArticleController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
    {
        var response = await _mediator.Send(command);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] GetPagedArticleQuery query)
    {
        var response = await _mediator.Send(query);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetPagedLast([FromQuery] GetPagedLastArticleQuery query)
    {
        var response = await _mediator.Send(query);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var response = await _mediator.Send(new GetArticleByIdQuery() { Id = id});
        return response.Match(x => Ok(x), e => Problem(e)); 
    }

    [Authorize(Policy = "AdminOrOwner")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(long id, [FromBody]EditArticleCommand command)
    {
        if (id != command.Id)
        {
            return Problem(Error.Failure("Id", "El id ingresado no es válido"));
        }
        var response = await _mediator.Send(command);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    [Authorize(Policy = "AdminOrOwner")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _mediator.Send(new DeleteArticleCommand() { Id = id });
        return response.Match(x => Ok(x), e => Problem(e));
    }


    [HttpPost("{id}/comments")]
    public async Task<IActionResult> Create(long id, [FromBody] CreateCommentCommand command)
    {
        if (id != command.ArticleId)
        {
            return Problem(Error.Failure("Id", "El id  del articulo ingresado no es válido"));
        }
        var response = await _mediator.Send(command);
        return response.Match(x => Ok(x), e => Problem(e));
    }
}