using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.Repository.SQLite.Generic;
using ArticlesService.UnitOfWork.SQLite.Core;

namespace ArticlesService.Repository.SQLite;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext ctx)
    {
        _context = ctx;
    }
}