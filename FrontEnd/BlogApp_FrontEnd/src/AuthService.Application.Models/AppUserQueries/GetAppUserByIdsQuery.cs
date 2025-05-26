using AuthService.Application.Models.AppUserDtos;

namespace AuthService.Application.Models.AppUserQueries;

public record GetAppUserByIdsQuery
{
    public List<long> Ids { get; set; }
}