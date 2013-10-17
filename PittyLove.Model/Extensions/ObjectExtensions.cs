using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PittyLove.Model.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Finds all object of the supplied type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="exclusionTypes">The exclusion types.</param>
        /// <returns></returns>
        public static IEnumerable<T> FindAll<T>(this object obj, params Type[] exclusionTypes) where T : class
        {
            var counter = new List<object>();
            foreach (var item in FindAll<T>(obj, counter, exclusionTypes))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Finds all object of the supplied type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="refcounter">The refcounter.</param>
        /// <param name="exclusionTypes">The exclusion types.</param>
        /// <returns></returns>
        private static IEnumerable<T> FindAll<T>(object obj, List<object> refcounter, params Type[] exclusionTypes) where T : class
        {
            if(!refcounter.Contains(obj))
            {
                refcounter.Add(obj);
                if (obj is T)
                {
                    yield return (obj as T);
                }

                var collection = obj as IEnumerable;
                if (collection != null)
                {
                    foreach (var item in collection)
                    {
                        foreach (var child in FindAll<T>(item, refcounter, exclusionTypes))
                        {
                            yield return child;
                        }
                    }
                }
                else
                {
                    foreach (var property in obj.GetType().GetProperties().Where(item => !IsTypeExcluded(item.PropertyType, exclusionTypes)))
                    {
                        var propertyValue = property.GetValue(obj, null);
                        if (propertyValue != null)
                        {
                            foreach (var item in FindAll<T>(propertyValue, refcounter, exclusionTypes))
                            {
                                yield return item;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is type excluded] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="exclusionTypes">The exclusion types.</param>
        /// <returns>
        ///   <c>true</c> if [is type excluded] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTypeExcluded(Type type, params Type[] exclusionTypes)
        {
            if (type == typeof(string) || type.IsValueType)
            {
                return true;
            }

            if (exclusionTypes != null
                  && exclusionTypes.Length > 0)
            {
                if (exclusionTypes.Contains(type))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
