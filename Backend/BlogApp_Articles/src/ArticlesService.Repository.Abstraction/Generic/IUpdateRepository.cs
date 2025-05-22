using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ArticlesService.Repository.Abstraction.Generic
{
    public interface IUpdateRepository<T> where T : class
    {
        void Update(T t);
        void Update(IEnumerable<T> t);
    }
}
