using System;
using System.Linq.Expressions;

namespace Services.Helpers
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, Boolean>> AndAlso<T>(this Expression<Func<T, Boolean>> left, Expression<Func<T, Boolean>> right)
        {
            var rightExp = new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body);
            if (rightExp == null)
            {
                throw new Exception();
            }

            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    rightExp
                ), left.Parameters);

            return combined;
        }
    }
}
