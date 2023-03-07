using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures.BaseInfrastructures
{
    public class BaseRepository<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected readonly DenimDbContext DenimDbContext;

        public BaseRepository(DenimDbContext denimDbContext)
        {
            DenimDbContext = denimDbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DenimDbContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType)
        {
            var typeQueryable = typeof(IQueryable<TEntity>);
            var argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            var props = orderColumn.Split('.');
            //var query = new List<TEntity>().AsQueryable<TEntity>();
            var type = typeof(TEntity);
            var arg = Expression.Parameter(type, "x");

            Expression expr = arg;

            foreach (var prop in props)
            {
                var pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi ?? throw new InvalidOperationException());
                type = pi.PropertyType;
            }

            var lambda = Expression.Lambda(expr, arg);
            var methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            var resultExp = Expression.Call(typeof(Queryable), methodName, new[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }

        public async Task<IQueryable<TEntity>> All()
        {
            try
            {
                var result = await DenimDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
                return result.AsQueryable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            try
            {
                DenimDbContext.Set<TEntity>().Remove(entity);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteRange(IEnumerable<TEntity> entities)
        {
            try
            {
                DenimDbContext.Set<TEntity>().RemoveRange(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            try
            {
                DenimDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                var res =  await DenimDbContext.Set<TEntity>().FindAsync(id);
                return res;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                return await DenimDbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TEntity> GetInsertedObjByAsync(TEntity entity)
        {
            try
            {
                await DenimDbContext.Set<TEntity>().AddAsync(entity);
                await SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<bool> InsertByAsync(TEntity entity)
        {
            try
            {
                await DenimDbContext.Set<TEntity>().AddAsync(entity);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertRangeByAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await DenimDbContext.Set<TEntity>().AddRangeAsync(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(TEntity entity)
        {
            try
            {
                var result = DenimDbContext.Set<TEntity>().Attach(entity);
                result.State = EntityState.Modified;
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateTr(TEntity entity)
        {
            try
            {
                var result = DenimDbContext.Set<TEntity>().Update(entity);
                result.State = EntityState.Modified;
                await SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateRangeByAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                DenimDbContext.Set<TEntity>().UpdateRange(entities);
                await SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> SaveChanges()
        {
            return await DenimDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DenimDbContext.Dispose();
        }
    }
}
