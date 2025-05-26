using ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Delete;
using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace ArticlesService.UnitTest.ArticlesTests;

public class DeleteArticleCommandHandlerTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<DeleteArticleCommandHandler>> _loggerMock;
    private readonly DeleteArticleCommandHandler _handler;

    public DeleteArticleCommandHandlerTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteArticleCommandHandler>>();

        _handler = new DeleteArticleCommandHandler(
            _articleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnUnit_WhenArticleIsDeleted()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = 1 };
        var existingArticle = new Article { Id = 1, Title = "Sample", Content = "Text" };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsNoTrackingAsync(x => x.Id == command.Id, null))
            .ReturnsAsync(existingArticle);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(Unit.Value, result.Value);
        _articleRepositoryMock.Verify(repo => repo.Remove(existingArticle), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenArticleDoesNotExist()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = 999 };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<Article, bool>>>(), null))
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
    public async Task Handle_ShouldReturnFailureError_WhenExceptionIsThrown()
    {
        // Arrange
        var command = new DeleteArticleCommand { Id = 1 };

        _articleRepositoryMock
            .Setup(repo => repo.SingleOrDefaultAsNoTrackingAsync(It.IsAny<Expression<Func<Article, bool>>>(), null))
            .Throws(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Exception", error.Code);
        Assert.Equal("Database error", error.Description);
    }
}