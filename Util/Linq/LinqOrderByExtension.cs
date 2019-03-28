using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq
{
    public static class LinqOrderByExtension
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return OrderByQueryableHelper<T>.OrderBy(queryable, propertyName, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, string propertyName)
        {
            return OrderByQueryableHelper<T>.OrderBy(queryable, propertyName, true);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> queryable, string propertyName)
        {
            return OrderByQueryableHelper<T>.ThenBy(queryable, propertyName, false);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> queryable, string propertyName)
        {
            return OrderByQueryableHelper<T>.ThenBy(queryable, propertyName, true);
        }



        static class OrderByQueryableHelper<T>
        {
            public static IOrderedQueryable<T> OrderBy(IQueryable<T> queryable, string propertyName, bool desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
            }

            public static IOrderedQueryable<T> ThenBy(IOrderedQueryable<T> queryable, string propertyName, bool desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return desc ? Queryable.ThenByDescending(queryable, keySelector) : Queryable.ThenBy(queryable, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(string propertyName)
            {
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                return keySelector;
            }
        }
    }
}
