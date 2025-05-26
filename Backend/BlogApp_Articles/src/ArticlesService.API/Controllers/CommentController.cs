using ArticlesService.API.Controllers.Base;
using ArticlesService.Application.Models.Dtos.CommentDtos;
using ArticlesService.Application.Models.Queries.CommentQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesService.API.Controllers;

/// <summary>
/// Controllador de comentarios
/// </summary>
[Route("api/[controller]")]
public class CommentController : ApiControllerBase
{
    private readonly ISender _mediator;

    public CommentController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Retorna la lista de comentarios asociados a un articulo
    /// </summary>
    /// <param name="articleId">Id del articulo</param>
    /// <returns>
    /// 200 OK con la lista de comentarios si se encuentra información.<br/>
    /// 404 Not Found si no se encuentran comentarios.<br/>
    /// 400/500 en caso de error o problema en la solicitud.
    /// </returns>
    [HttpGet("[action]/{articleId}")]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByArticleId(long articleId)
    {
        var response = await _mediator.Send(new GetCommentByArticleIdQuery() { ArticuloId = articleId });
        return response.Match(x => Ok(x), e => Problem(e));
    }
}