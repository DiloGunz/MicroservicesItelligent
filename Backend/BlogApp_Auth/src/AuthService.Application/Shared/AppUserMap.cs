using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserDtos;
using AuthService.Domain;
using AutoMapper;

namespace AuthService.Application.Shared;

public class AppUserMap : Profile
{
	public AppUserMap()
	{
		CreateMap<RegisterAppUserCommand, AppUser>();
        CreateMap<AppUser, AppUserDto>();
    }
}