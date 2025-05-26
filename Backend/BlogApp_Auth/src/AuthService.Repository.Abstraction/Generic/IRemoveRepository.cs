using System.Linq.Expressions;

namespace AuthService.Repository.Abstraction.Generic
{
    public interface IRemoveRepository<T> where T : class
    {
        /// <summary>
        /// Remove as logic level
        /// </summary>
        /// <param name="t"></param>
        void Remove(T t);

        /// Remove collection as logic level
        void Remove(IEnumerable<T> t);


        // remove at new version
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
        int ExecuteDelete(Expression<Func<T, bool>> predicate);
    }
}
