using AuthService.Application.Login;
using AuthService.Application.Models.Login;
using AuthService.Domain;
using AuthService.Repository.Abstraction;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;

namespace AuthServices.Tests;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAppUserRepository> _usuarioRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IAppUserRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _handler = new LoginCommandHandler(
            _usuarioRepositoryMock.Object,
            _configurationMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand { Username = "testuser", Password = "1234" };

        _usuarioRepositoryMock
            .Setup(r => r.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<AppUser, bool>>>(), null))
            .ReturnsAsync((AppUser)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Usuario.Username", error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new LoginCommand { Username = "testuser", Password = "wrongpassword" };

        var user = new AppUser
        {
            Id = 1,
            Username = "TESTUSER",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Role = "User"
        };

        _usuarioRepositoryMock
            .Setup(r => r.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<AppUser, bool>>>(), null))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Usuario.PasswordHash", error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand { Username = "testuser", Password = "password123" };

        var user = new AppUser
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "admin"
        };

        _usuarioRepositoryMock
            .Setup(r => r.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<AppUser, bool>>>(), null))
            .ReturnsAsync(user);

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_jwt_key_1234567890_AA");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("MyIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("MyAudience");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.True(result.Value.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.Token));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenJwtKeyIsMissing()
    {
        // Arrange
        var command = new LoginCommand { Username = "testuser", Password = "password123" };

        var user = new AppUser
        {
            Id = 1,
            Username = "TESTUSER",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "Admin"
        };

        _usuarioRepositoryMock
            .Setup(r => r.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<AppUser, bool>>>(), null))
            .ReturnsAsync(user);

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns((string)null); // Simula key ausente

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Llave de JWT inválido", ex.Message);
    }
}
