using ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Create;
using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ArticlesService.Repository.Abstraction;
using ArticlesService.SharedKernel.Collection;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.GetPaged;

public class GetPagedArticleQueryHandler : IRequestHandler<GetPagedArticleQuery, ErrorOr<DataCollection<ArticleListDto>>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetPagedArticleQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPagedArticleQueryHandler(IArticleRepository articleRepository, ILogger<GetPagedArticleQueryHandler> logger, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<DataCollection<ArticleListDto>>> Handle(GetPagedArticleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _articleRepository.GetPagedAsync(
                request.Page,
                request.Take,
                x => x.OrderByDescending(y => y.CreatedAt));

            return _mapper.Map<DataCollection<ArticleListDto>>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}