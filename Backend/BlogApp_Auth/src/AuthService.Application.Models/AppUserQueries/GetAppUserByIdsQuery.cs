using AuthService.Application.Models.AppUserDtos;
using ErrorOr;
using MediatR;

namespace AuthService.Application.Models.AppUserQueries;

public record GetAppUserByIdsQuery : IRequest<ErrorOr<List<AppUserDto>>>
{
    public List<long> Ids { get; set; }
}