using HPTA.Common.Extensions;
using HPTA.Data;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HPTA.Repositories.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly HPTADbContext _hptaDbContext;
        protected readonly DbSet<T> dbset;

        public Repository(HPTADbContext hptaDbContext)
        {
            _hptaDbContext = hptaDbContext;
            if (_hptaDbContext == null)
            {
                throw new Exception("Invalid hpta db context.");
            }

            this.dbset = _hptaDbContext.Set<T>();
        }

        /// <summary>
        /// The Add an entity
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public virtual void Add(T entity, bool suppressDivIdMapping = false)
        {
            this.dbset.Add(entity);
        }

        /// <summary>
        /// The Update an entity
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(T entity)
        {
            var entry = _hptaDbContext.Entry(entity);
            this.dbset.Attach(entity);
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// The Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            var entry = _hptaDbContext.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        /// <summary>
        /// To get the entities by provided condition.
        /// </summary>
        /// <param name="predicate">The conditions to filter with.</param>
        /// <returns>The list of entities</returns>GetAllAsync
        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                return this.Include(this.dbset.Where(this.ApplyFilters(predicate)), includes);
            }

            return this.dbset.Where(this.ApplyFilters(predicate));
        }

        /// <summary>
        /// To get the entities by provided condition asynchronously.
        /// </summary>
        /// <param name="predicate">The conditions to filter with.</param>
        /// <param name="includes">The 0 or more navigation properties to include for EF eager loading.</param>
        /// <returns>The list of entities</returns>
        public async Task<List<T>> GetByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return await GetBy(predicate, includes).ToListAsync();
        }

        /// <summary>
        /// To get all entities.
        /// </summary>
        /// <param name="includes">The 0 or more navigation properties to include for EF eager loading.</param>
        /// <returns>The list of entities</returns>
        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                return this.Include(this.dbset.Where(this.ApplyFilters()), includes);
            }

            return this.dbset.Where(this.ApplyFilters());
        }

        /// <summary>
        /// To get all entities asynchronously.
        /// </summary>
        /// <param name="includes">The 0 or more navigation properties to include for EF eager loading.</param>
        /// <returns>The list of entities</returns>
        public virtual async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            return await this.GetAll(includes).ToListAsync();
        }

        /// <summary>
        /// To determine the existance of an entity by the provided condition.
        /// </summary>
        /// <param name="predicate">The condition to filter with.</param>
        /// <returns>Either true or false as per the match.</returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.AnyAsync(predicate).Result;
        }

        /// <summary>
        /// To determine the existance of an entity by the provided condition asynchronously.
        /// </summary>
        /// <param name="predicate">The condition to filter with.</param>
        /// <returns>Either true or false as per the match.</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.dbset.AnyAsync(this.ApplyFilters(predicate)).ConfigureAwait(false);
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await _hptaDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// To apply the filtes to provided expression.
        /// </summary>
        /// <param name="expression">The expression to which the filter needs to be applied.</param>
        /// <returns>Filtered expression.</returns>
        protected Expression<Func<T, bool>> ApplyFilters(Expression<Func<T, bool>> expression = null)
        {
            // By default consider all.
            Expression<Func<T, bool>> filter = x => true;

            return expression == null ? filter : expression.And(filter);
        }

        private IQueryable<T> Include(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                query = this.IncludeProperty(query, include);
            }

            return query;
        }

        private IQueryable<T> IncludeProperty<TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> include) => query.Include(include);

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="dispose">The Dispose</param>
        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                // Nothing to dispose here.
            }
        }

        public async Task DeleteByAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await this.dbset.FirstOrDefaultAsync(this.ApplyFilters(predicate));
            dbset.Remove(entity);
        }
    }
}
