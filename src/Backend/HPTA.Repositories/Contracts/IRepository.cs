using HPTA.Data;
using System.Linq.Expressions;

namespace HPTA.Repositories.Contracts
{
    public interface IRepository<T> : IDisposable where T : class, IEntity
    {
        /// <summary>
        /// To get all entities.
        /// </summary>
        /// <param name="includes">The 0 or more navigation properties to include for EF eager loading.</param>
        /// <returns>The list of entities</returns>
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// To get all entities asynchronously.
        /// </summary>
        /// <param name="includes">The 0 or more navigation properties to include for EF eager loading.</param>
        /// <returns>The list of entities</returns>
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// To get the entities by provided condition.
        /// </summary>
        /// <param name="predicate">The conditions to filter with.</param>
        /// <returns>The list of entities</returns>
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// To determine the existance of an entity by the provided condition.
        /// </summary>
        /// <param name="predicate">The condition to filter with.</param>
        /// <returns>Either true or false as per the match.</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// To determine the existance of an entity by the provided condition asynchronously.
        /// </summary>
        /// <param name="predicate">The condition to filter with.</param>
        /// <returns>Either true or false as per the match.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// The Add an entity
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="suppressOrgIdMapping">
        void Add(T entity, bool suppressOrgIdMapping = false);

        /// <summary>
        /// The Update an entity
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// The Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        Task DeleteByAsync(Expression<Func<T, bool>> predicate);

        Task<int> SaveAsync();
    }
}
