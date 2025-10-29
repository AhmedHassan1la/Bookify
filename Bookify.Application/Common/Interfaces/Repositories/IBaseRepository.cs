using System.Linq.Expressions;

namespace Bookify.Application.Common.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T? GetById(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        T? FirstOrDefault(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAllQueryable();
        IQueryable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
