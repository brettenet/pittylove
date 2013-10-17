using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PittyLove.Model.Extensions;

namespace PittyLove.Model
{
    public class EfRepository<T> : IRepository<T> where T : class, IEntity
    {
        #region Private Data Members

        /// <summary>
        /// Object Set
        /// </summary>
        private readonly DbSet<T> _dbSet;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected internal DbContext Context { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        public EfRepository(DbContext context)
        {
            _dbSet = context.Set<T>();
            Context = context;
        }

        #endregion

        #region IRepository<T> Members

        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            return _dbSet.WithIncludes(includes).FirstOrDefault(entity => entity.Id == id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return _dbSet.WithIncludes(includes);
        }

        /// <summary>
        /// Finds the where.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public IEnumerable<T> FindWhere(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return _dbSet.WithIncludes(includes).Where(predicate);
        }

        /// <summary>
        /// Adds the specified new entity.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        public void Add(T newEntity)
        {
            //This is just a hack for the presentation.
            //For simplicity I just hard code the image path 
            //for all share a dog posts.
            var pitbull = newEntity as Pitbull;
            if (pitbull != null)
            {
                pitbull.ImageUrl = "/images/pits/puddlesbone.jpg";
            }

            _dbSet.Add(newEntity);
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Remove(T entity)
        {
            BeforeRemove(entity);
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Ases the queryable.
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        #endregion

        #region Protected Methods

        public DbSet<T> Set
        {
            get
            {
                return _dbSet;
            }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Called before the entity is removed from the context.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void BeforeRemove(T entity)
        {

        }

        #endregion
    }
}
