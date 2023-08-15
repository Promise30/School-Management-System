using System.Linq.Expressions;

namespace School_Management.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null);
        Task<T> Get(
            Expression<Func<T, bool>> expression,
            List<string> includes = null);
        Task Insert(T entity);
        Task Delete(int id);

        void Update(T entity);
    }
}
