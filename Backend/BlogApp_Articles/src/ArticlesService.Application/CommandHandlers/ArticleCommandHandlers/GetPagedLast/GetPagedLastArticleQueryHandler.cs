using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ArticlesService.Repository.Abstraction;
using ArticlesService.SharedKernel.Collection;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.GetPagedLast;

public class GetPagedLastArticleQueryHandler : IRequestHandler<GetPagedLastArticleQuery, ErrorOr<DataCollection<ArticleDetailsDto>>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetPagedLastArticleQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPagedLastArticleQueryHandler(IArticleRepository articleRepository, ILogger<GetPagedLastArticleQueryHandler> logger, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<DataCollection<ArticleDetailsDto>>> Handle(GetPagedLastArticleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _articleRepository.GetPagedAsync(
                request.Page,
                request.Take,
                x => x.OrderByDescending(y => y.CreatedAt));

            return _mapper.Map<DataCollection<ArticleDetailsDto>>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}