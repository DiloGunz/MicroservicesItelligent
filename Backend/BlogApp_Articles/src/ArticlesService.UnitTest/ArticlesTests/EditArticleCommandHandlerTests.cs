using ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Edit;
using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticlesService.UnitTest.ArticlesTests;

public class EditArticleCommandHandlerTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<EditArticleCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EditArticleCommandHandler _handler;

    public EditArticleCommandHandlerTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<EditArticleCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new EditArticleCommandHandler(
            _articleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnArticleId_WhenArticleIsEdited()
    {
        // Arrange
        var command = new EditArticleCommand { Id = 1, Title = "New Title", Content = "Updated Content" };
        var article = new Article { Id = 1, Title = "Old Title", Content = "Old Content" };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsync(x => x.Id == command.Id, null))
            .ReturnsAsync(article);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(article.Id, result.Value);
        _mapperMock.Verify(m => m.Map(command, article), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenArticleDoesNotExist()
    {
        // Arrange
        var command = new EditArticleCommand { Id = 99 };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsync(x => x.Id == command.Id, null))
            .ReturnsAsync((Article)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Article", error.Code);
        Assert.Equal("El articulo seleccionado no existe.", error.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureError_WhenExceptionThrown()
    {
        // Arrange
        var command = new EditArticleCommand { Id = 1 };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsync(x => x.Id == command.Id, null))
            .Throws(new Exception("Database failure"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Exception", error.Code);
        Assert.Equal("Database failure", error.Description);
    }
}