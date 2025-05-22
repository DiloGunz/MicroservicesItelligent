using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction.Generic;

namespace ArticlesService.Repository.Abstraction;

public interface IArticleRepository : 
    IPagedRepository<Article>,
    ICreateRepository<Article>,
    IUpdateRepository<Article>,
    IRemoveRepository<Article>,
    IReadRepository<Article>
{
}