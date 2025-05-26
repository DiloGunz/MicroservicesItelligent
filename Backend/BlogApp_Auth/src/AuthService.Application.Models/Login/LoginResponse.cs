namespace AuthService.Application.Models.Login;

public record LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}