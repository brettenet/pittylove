using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PittyLove.Model
{
    /// <summary>
    /// core repository contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets the specific entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        T GetById(int id, params Expression<Func<T, object>>[] includes);
        /// <summary>
        /// Gets all instances of type T.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        /// <summary>
        /// Finds items by the supplied predicate
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        IEnumerable<T> FindWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        /// <summary>
        /// Adds the specified new entity.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        void Add(T newEntity);
        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(T entity);
        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable();
    }
}