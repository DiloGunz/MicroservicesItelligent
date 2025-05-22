using ArticlesService.Application.Models.Dtos.CommentDtos;
using ArticlesService.Application.Models.Queries.CommentQueries;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.CommentCommandhandlers.BetByArticle;

public class GetCommentByArticleIdQueryHandler : IRequestHandler<GetCommentByArticleIdQuery, ErrorOr<List<CommentDto>>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILogger<GetCommentByArticleIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCommentByArticleIdQueryHandler(ICommentRepository commentRepository, ILogger<GetCommentByArticleIdQueryHandler> logger, IMapper mapper)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<ErrorOr<List<CommentDto>>> Handle(GetCommentByArticleIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _commentRepository.GetAllAsNoTrackingAsync(x => x.ArticleId == request.ArticuloId, include: x => x.Include(y => y.Article));
            var response = _mapper.Map<List<CommentDto>>(data);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}