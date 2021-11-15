using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LoginForm.Data.Extensions
{
    public class LinqDynamicExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> GetExpression<T>(List<LinqDynamicExpressionHelper.LinqDynamicFilter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "parm");

            // Store the result of a calculated Expression
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression<T>(param, filters[0]); // Create expression from a single instance
            }
            else if (filters.Count == 2)
            {
                exp = GetExpression<T>(param, filters[0], filters[1]); // Create expression that utilizes AndAlso mentality
            }
            else
            {
                // Loop through filters until we have created an expression for each
                while (filters.Count > 0)
                {
                    // Grab initial filters remaining in our List
                    var f1 = filters[0];
                    var f2 = filters[1];

                    // Check if we have already set our Expression
                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]); // First iteration through our filters
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1])); // Add to our existing expression

                    filters.Remove(f1);
                    filters.Remove(f2);


                    // Odd number, handle this seperately
                    if (filters.Count == 1)
                    {
                        // Pass in our existing expression and our newly created expression from our last remaining filter
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));

                        // Remove filter to break out of while loop
                        filters.RemoveAt(0);

                    }
                }
            }

            if (exp != null) return Expression.Lambda<Func<T, bool>>(exp, param);
            return null;
        }

        private static Expression GetExpression<T>(ParameterExpression param, LinqDynamicExpressionHelper.LinqDynamicFilter filter)
        {
            if (typeof(T).GetProperty(filter.PropertyName.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
            {
                MemberExpression member = Expression.Property(param, filter.PropertyName);

                // The value you want to evaluate
                ConstantExpression constant = Expression.Constant(filter.SearchValue);

                // Determine how we want to apply the expression
                switch (filter.Operator)
                {
                    case LinqDynamicExpressionHelper.LinqDynamicOperator.Equals:
                        return Expression.Equal(member, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.Contains:
                        return Expression.Call(member, containsMethod, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.GreaterThan:
                        return Expression.GreaterThan(member, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.GreaterThanOrEqual:
                        return Expression.GreaterThanOrEqual(member, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.LessThan:
                        return Expression.LessThan(member, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.LessThanOrEqualTo:
                        return Expression.LessThanOrEqual(member, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.StartsWith:
                        return Expression.Call(member, startsWithMethod, constant);

                    case LinqDynamicExpressionHelper.LinqDynamicOperator.EndsWith:
                        return Expression.Call(member, endsWithMethod, constant);
                }
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, LinqDynamicExpressionHelper.LinqDynamicFilter filter1, LinqDynamicExpressionHelper.LinqDynamicFilter filter2)
        {
            Expression result1 = GetExpression<T>(param, filter1);
            Expression result2 = GetExpression<T>(param, filter2);
            return Expression.AndAlso(result1, result2);
        }
    }
}
