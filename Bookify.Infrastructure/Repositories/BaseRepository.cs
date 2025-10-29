using Bookify.Application.Common.Interfaces;
using Bookify.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookify.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _context;

        public BaseRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();


        public void Add(T entity) => _context.Set<T>().Add(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);


        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);


        public IQueryable<T> GetAllQueryable() => _context.Set<T>();

        public IQueryable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public T? GetById(int id) => _context.Set<T>().Find(id);

        public T? FirstOrDefault(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

        public bool Any(Expression<Func<T, bool>> predicate) => _context.Set<T>().Any(predicate);

    }
}
