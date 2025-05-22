using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;

namespace ArticlesService.UnitOfWork.Repository;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
	public UnitOfWorkRepository(IArticleRepository articleRepository, ICommentRepository commentRepository)
	{
		ArticleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
		CommentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
	}

    public IArticleRepository ArticleRepository { get; }

    public ICommentRepository CommentRepository { get; }
}