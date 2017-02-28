using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// Linq Where查询条件的扩展
    /// 
    /// if(string.IsNullOrEmpty(name) == false)
    /// result = source.Where(p => p.Name.Contains(name));
    /// 
    /// 对于这样的条件我们使用本扩展进行编写, 增强代码易读性
    /// .WhereIf(p => p.Name.Contains(name), string.IsNullOrEmpty(name) == false)
    /// </summary>
    public static class WhereIfExtension
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}