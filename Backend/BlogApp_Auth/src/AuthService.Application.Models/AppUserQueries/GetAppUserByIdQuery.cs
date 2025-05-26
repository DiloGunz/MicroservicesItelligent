using AuthService.Application.Models.AppUserDtos;
using ErrorOr;
using MediatR;

namespace AuthService.Application.Models.AppUserQueries;

public record GetAppUserByIdQuery : IRequest<ErrorOr<AppUserDto>>
{
    public long Id { get; set; }
}