using ArticlesService.SharedKernel.Collection;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ArticlesService.Repository.Abstraction.Generic;

public interface IPagedRepository<T> where T : class
{
    Task<DataCollection<T>> GetPagedAsync(
        int page,
        int take,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null
    );

    DataCollection<T> GetPaged(
        int page,
        int take,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null
    );
}