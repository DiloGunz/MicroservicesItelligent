using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.Repository.SQLite.Generic;
using ArticlesService.UnitOfWork.SQLite.Core;

namespace ArticlesService.Repository.SQLite;

public class ArticleRepository : 
    GenericRepository<Article>,
    IArticleRepository
{
    public ArticleRepository(AppDbContext ctx)
    {
        _context = ctx;
    }
}