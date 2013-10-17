using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PittyLove.Model.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Simple extension method used to stack includeds in Linq to entity queries.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public static IQueryable<T> WithIncludes<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                queryable = includes.Aggregate(queryable, (current, include) => current.Include(include));
            }

            return queryable;
        }
    }
}
