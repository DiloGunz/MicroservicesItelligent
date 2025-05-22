using ArticlesService.Repository.Abstraction;

namespace ArticlesService.UnitOfWork.Abstraction;

public interface IUnitOfWorkRepository
{
    IArticleRepository ArticleRepository { get; }
    ICommentRepository CommentRepository { get; }
}