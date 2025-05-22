namespace AuthService.Application.Models.AppUserDtos;

public record AppUserDto
{
    public long Id { get; set; }
    public string Username { get; set; }
}