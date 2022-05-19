using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace kiosk_solution.Data.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync<TKey>(TKey id);
        
        void Insert(TEntity entity);
        
        Task InsertAsync(TEntity entity);
        TEntity GetByID(Guid id);
        
        void Delete(Guid id);
        
        void Delete(TEntity entity);
        
        void Update(TEntity entity);
    }
}