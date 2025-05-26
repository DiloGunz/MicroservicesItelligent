using ArticlesService.Application.CommandHandlers.CommentCommandhandlers.Create;
using ArticlesService.Application.Models.Commands.CommentCommands;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticlesService.UnitTest.CommentTests;

public class CreateCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateCommentCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateCommentCommandHandler _handler;

    public CreateCommentCommandHandlerTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateCommentCommandHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateCommentCommandHandler(
            _commentRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnCommentId_WhenSuccess()
    {
        // Arrange
        var command = new CreateCommentCommand
        {
            Content = "Nice article!",
            ArticleId = 1
        };

        var comment = new Comment
        {
            Id = 123,
            Content = command.Content,
            ArticleId = command.ArticleId
        };

        _mapperMock.Setup(m => m.Map<Comment>(command)).Returns(comment);
        _commentRepositoryMock.Setup(repo => repo.AddAsync(comment)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(comment.Id, result.Value);
        _mapperMock.Verify(m => m.Map<Comment>(command), Times.Once);
        _commentRepositoryMock.Verify(repo => repo.AddAsync(comment), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureError_WhenExceptionThrown()
    {
        // Arrange
        var command = new CreateCommentCommand
        {
            Content = "Failure test",
            ArticleId = 999
        };

        _mapperMock.Setup(m => m.Map<Comment>(command)).Throws(new Exception("Mapping error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Exception", error.Code);
        Assert.Equal("Mapping error", error.Description);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Mapping error")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}