using AuthService.API.Controllers.Base;
using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.AppUserQueries;
using AuthService.Application.Models.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers;

/// <summary>
/// Controlador de Autenticacion
/// </summary>
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly ISender _mediator;

	public AuthController(IMediator mediator)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}

	/// <summary>
	/// Retorna datos del usuario autenticado
	/// </summary>
	/// <returns></returns>
	[Authorize]
	[HttpGet("me")]
    [ProducesResponseType(typeof(AppUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
	{
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		var query = new GetAppUserByIdQuery
		{
			Id = Convert.ToInt64(userId)
		};
		var response = await _mediator.Send(query);
		return response.Match(x => Ok(x), e => Problem(e));
	}

	/// <summary>
	/// Registra un nuevo usuario en la base de datos
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("[action]")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterAppUserCommand command)
	{
		var result = await _mediator.Send(command);
		return result.Match(x => Ok(x), errors => Problem(errors));
	}

	/// <summary>
	/// login
	/// </summary>
	/// <param name="command">DTO de usuario y contraseña</param>
	/// <returns></returns>
	[HttpPost("[action]")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
	{
        var result = await _mediator.Send(command);
        return result.Match(x => Ok(x), errors => Problem(errors));
    }

	/// <summary>
	/// Retorna una lista de usuario a partir de una lista de ids
	/// </summary>
	/// <param name="query">Contiene la lista de ids que se van a buscar</param>
	/// <returns></returns>
	[Authorize]
	[HttpPost("[action]")]
    [ProducesResponseType(typeof(List<AppUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByIds([FromBody] GetAppUserByIdsQuery query)
	{
		var response = await _mediator.Send(query);
		return response.Match(x => Ok(x), e => Problem(e));
	}
}