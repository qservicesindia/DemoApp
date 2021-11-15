using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace LoginForm.Data.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WithIncludes<T>(this IQueryable<T> query, Expression<Func<T, object>>[] selectors) where T : class
        {
            foreach (var selector in selectors)
            {
                string path = new PropertyPathVisitor().GetPropertyPath(selector);
                query = query.Include(path);
            }

            return query;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy<T, int>((item) => rnd.Next());
        }
    }

    public class PropertyPathVisitor : ExpressionVisitor
    {
        private Stack<string> _stack;

        public string GetPropertyPath(Expression expression)
        {
            _stack = new Stack<string>();
            Visit(expression);
            return _stack
                .Aggregate(
                    new StringBuilder(),
                    (sb, name) =>
                        (sb.Length > 0 ? sb.Append(".") : sb).Append(name))
                .ToString();
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (_stack != null)
                _stack.Push(expression.Member.Name);
            return base.VisitMember(expression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (IsLinqOperator(expression.Method))
            {
                for (int i = 1; i < expression.Arguments.Count; i++)
                {
                    Visit(expression.Arguments[i]);
                }
                Visit(expression.Arguments[0]);
                return expression;
            }
            return base.VisitMethodCall(expression);
        }

        private static bool IsLinqOperator(MethodInfo method)
        {
            if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
                return false;
            return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
        }
    }
}
