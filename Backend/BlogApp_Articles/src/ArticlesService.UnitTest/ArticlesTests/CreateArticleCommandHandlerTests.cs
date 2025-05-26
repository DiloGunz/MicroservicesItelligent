using ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Create;
using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticlesService.UnitTest.ArticlesTests;

public class CreateArticleCommandHandlerTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateArticleCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly CreateArticleCommandHandler _handler;

    public CreateArticleCommandHandlerTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateArticleCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateArticleCommandHandler(
            _articleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnArticleId_WhenSuccess()
    {
        // Arrange
        var command = new CreateArticleCommand
        {
            Title = "Test Title",
            Content = "Test Content"
        };

        var article = new Article { Id = 1, Title = "Test Title", Content = "Test Content" };

        _mapperMock.Setup(m => m.Map<Article>(command)).Returns(article);
        _articleRepositoryMock.Setup(repo => repo.AddAsync(article)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(article.Id, result.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenExceptionThrown()
    {
        // Arrange
        var command = new CreateArticleCommand
        {
            Title = "Test Title",
            Content = "Test Content"
        };

        _mapperMock.Setup(m => m.Map<Article>(command)).Throws(new Exception("Mapping failed"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Exception", error.Code);
        Assert.Equal("Mapping failed", error.Description);
    }
}
