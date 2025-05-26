using AuthService.API.Controllers.Base;
using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserQueries;
using AuthService.Application.Models.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly ISender _mediator;

	public AuthController(IMediator mediator)
	{
		_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}

	[Authorize]
	[HttpGet("me")]
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

	[HttpPost("[action]")]
	public async Task<IActionResult> Register([FromBody] RegisterAppUserCommand command)
	{
		var result = await _mediator.Send(command);
		return result.Match(x => Ok(x), errors => Problem(errors));
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Login([FromBody] LoginCommand command)
	{
        var result = await _mediator.Send(command);
        return result.Match(x => Ok(x), errors => Problem(errors));
    }

	[Authorize]
	[HttpPost("[action]")]
	public async Task<IActionResult> GetByIds([FromBody] GetAppUserByIdsQuery query)
	{
		var response = await _mediator.Send(query);
		return response.Match(x => Ok(x), e => Problem(e));
	}
}