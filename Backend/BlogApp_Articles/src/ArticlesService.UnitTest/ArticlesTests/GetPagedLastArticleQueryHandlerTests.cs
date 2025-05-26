using ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.GetPagedLast;
using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.SharedKernel.Collection;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArticlesService.UnitTest.ArticlesTests;

public class GetPagedLastArticleQueryHandlerTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly Mock<ILogger<GetPagedLastArticleQueryHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPagedLastArticleQueryHandler _handler;

    public GetPagedLastArticleQueryHandlerTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _loggerMock = new Mock<ILogger<GetPagedLastArticleQueryHandler>>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetPagedLastArticleQueryHandler(
            _articleRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedPagedResult_WhenSuccess()
    {
        // Arrange
        var query = new GetPagedLastArticleQuery { Page = 1, Take = 2 };

        var domainArticles = new DataCollection<Article>
        {
            Items = new List<Article>
            {
                new Article { Id = 1, Title = "A", CreatedAt = DateTime.UtcNow },
                new Article { Id = 2, Title = "B", CreatedAt = DateTime.UtcNow.AddMinutes(-1) }
            },
            Total = 2,
            Page = 1,
            Pages = 1
        };

        var dtoArticles = new DataCollection<ArticleDetailsDto>
        {
            Items = new List<ArticleDetailsDto>
            {
                new ArticleDetailsDto { Id = 1, Title = "A" },
                new ArticleDetailsDto { Id = 2, Title = "B" }
            },
            Total = 2,
            Page = 1,
            Pages = 1
        };

        _articleRepositoryMock
            .Setup(repo => repo.GetPagedAsync(
                query.Page,
                query.Take,
                It.IsAny<Func<IQueryable<Article>, 
                IOrderedQueryable<Article>>>(),
                null, null))
            .ReturnsAsync(domainArticles);

        _mapperMock
            .Setup(m => m.Map<DataCollection<ArticleDetailsDto>>(domainArticles))
            .Returns(dtoArticles);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(2, result.Value.Total);
        Assert.Collection(result.Value.Items,
            item => Assert.Equal("A", item.Title),
            item => Assert.Equal("B", item.Title));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var query = new GetPagedLastArticleQuery { Page = 1, Take = 5 };

        _articleRepositoryMock
            .Setup(repo => repo.GetPagedAsync(
                query.Page,
                query.Take,
                It.IsAny<Func<IQueryable<Article>, IOrderedQueryable<Article>>>(), null, null))
            .Throws(new Exception("Database unavailable"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        var error = result.FirstError;
        Assert.Equal("Exception", error.Code);
        Assert.Equal("Database unavailable", error.Description);
    }
}
