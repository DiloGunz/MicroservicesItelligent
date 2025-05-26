using ArticlesService.API.Controllers.Base;
using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Application.Models.Commands.CommentCommands;
using ArticlesService.Application.Models.Dtos.CommentDtos;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesService.API.Controllers;

/// <summary>
/// Controlador de artículos
/// </summary>
[Route("api/articles")]
public class ArticleController : ApiControllerBase
{
    private readonly ISender _mediator;

    public ArticleController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Registra un nuevo artículo en la base de datos
    /// </summary>
    /// <param name="command">DTP de articulo</param>
    /// <returns>
    /// 200 OK.
    /// 401 Error en la validacion de datos
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
    {
        var response = await _mediator.Send(command);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    /// <summary>
    /// Retorna una lista paginada de articulos
    /// </summary>
    /// <param name="query">información de consulta</param>
    /// <returns>
    /// 200 OK con la lista paginada de articulos
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] GetPagedArticleQuery query)
    {
        var response = await _mediator.Send(query);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    /// <summary>
    /// Retorna una lista paginada de ultimos articulos registrados
    /// </summary>
    /// <param name="query">parametros de consulta</param>
    /// <returns>
    /// 200 OK con la lista paginada de articulos
    /// </returns>
    [HttpGet("[action]")]
    public async Task<IActionResult> GetPagedLast([FromQuery] GetPagedLastArticleQuery query)
    {
        var response = await _mediator.Send(query);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    /// <summary>
    /// Retorna un Articulo mediante su ID
    /// </summary>
    /// <param name="id">id del artidulo</param>
    /// <returns>
    /// 200 OK con informacion del articulo
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var response = await _mediator.Send(new GetArticleByIdQuery() { Id = id});
        return response.Match(x => Ok(x), e => Problem(e)); 
    }

    /// <summary>
    /// Edita un artículo registrado
    /// </summary>
    /// <param name="id">id del articulo</param>
    /// <param name="command">DTO con ifnormacion a modificar</param>
    /// <returns>
    /// 200 OK si la actualizacion es válida
    /// 401 Probelmas de validacion
    /// 403 No autorizado (en caso de no sea admin ni el propietario del registro)
    /// </returns>
    [Authorize(Policy = "AdminOrOwner")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Edit(long id, [FromBody]EditArticleCommand command)
    {
        if (id != command.Id)
        {
            return Problem(Error.Failure("Id", "El id ingresado no es válido"));
        }
        var response = await _mediator.Send(command);
        return response.Match(x => Ok(x), e => Problem(e));
    }

    /// <summary>
    /// Elimina un articulo registrado
    /// </summary>
    /// <param name="id">id del articulo</param>
    /// <returns>
    /// 200 OK si la actualizacion es válida
    /// 401 Probelmas de validacion
    /// 403 No autorizado (en caso de no sea admin ni el propietario del registro)
    /// </returns>
    [Authorize(Policy = "AdminOrOwner")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _mediator.Send(new DeleteArticleCommand() { Id = id });
        return response.Match(x => Ok(x), e => Problem(e));
    }


    /// <summary>
    /// Agrega un comentario a un articulo existente
    /// </summary>
    /// <param name="id">id del articulo</param>
    /// <param name="command">DTO con datos del comentario</param>
    /// <returns>
    /// 200 OK con el id del comentario registrado
    /// </returns>
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