using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HRMS.ServiceInterfaces.BaseInterfaces
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType);
        Task<IQueryable<TEntity>> All();
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> FindByIdAsync(int id);
        Task<TEntity> GetInsertedObjByAsync(TEntity entity);
        Task<bool> InsertByAsync(TEntity entity);
        Task<bool> InsertRangeByAsync(IEnumerable<TEntity> entities);
        Task<bool> Update(TEntity entity);
        Task<bool> UpdateRangeByAsync(IEnumerable<TEntity> entities);
        Task<bool> Delete(TEntity entity);
        Task<bool> DeleteRange(IEnumerable<TEntity> entities);
    }
}
