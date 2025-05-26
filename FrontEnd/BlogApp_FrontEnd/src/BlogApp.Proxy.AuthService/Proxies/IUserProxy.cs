using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.AppUserQueries;
using BlogApp.Shared.Responses;

namespace BlogApp.Proxy.AuthService.Proxies;

public interface IUserProxy
{
    Task<SimpleResponse<long>> RegisterAsync(RegisterAppUserCommand command);
    Task<SimpleResponse<AppUserDto>> GetMeAsync();
    Task<List<AppUserDto>> GetByIdsAsync(GetAppUserByIdsQuery query);
}