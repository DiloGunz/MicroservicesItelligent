using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction.Generic;

namespace ArticlesService.Repository.Abstraction;

public interface ICommentRepository : 
    IReadRepository<Comment>,
    ICreateRepository<Comment>
{
}