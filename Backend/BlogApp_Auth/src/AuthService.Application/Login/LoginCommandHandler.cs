using AuthService.Application.Models.Login;
using AuthService.Domain;
using AuthService.Repository.Abstraction;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Application.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResponse>>
{
    private readonly IAppUserRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(IAppUserRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existeUsuario = await _usuarioRepository.SingleOrDefaultAsNoTrackingAsync(x=>x.Username == request.Username.ToUpper());

        if (existeUsuario == null)
        {
            return Error.Validation("Usuario.Username", "El username ingresado no existe.");
        }

        var passwordValido = BCrypt.Net.BCrypt.Verify(request.Password, existeUsuario.PasswordHash);

        if (!passwordValido)
        {
            return Error.Validation("Usuario.PasswordHash", "La contraseña ingresada no es válida.");
        }

        var response = new LoginResponse
        {
            Success = true,
            Token = GenerarJwtToken(existeUsuario)
        };

        return response;
    }

    private string GenerarJwtToken(AppUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException("Llave de JWT inválido")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Username),
            new Claim("role", user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}