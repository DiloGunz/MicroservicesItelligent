using ErrorOr;
using MediatR;

namespace AuthService.Application.Models.Login;

public record LoginCommand : IRequest<ErrorOr<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}