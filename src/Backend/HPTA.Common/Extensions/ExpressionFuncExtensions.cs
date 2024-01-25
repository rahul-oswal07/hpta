using HPTA.Common.Helpers;
using System.Linq.Expressions;

namespace HPTA.Common.Extensions
{
    public static class ExpressionFuncExtensions
    {
        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// Combines two given expressions by using the AND semantics.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="first">The first part of the expression.</param>
        /// <param name="second">The second part of the expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }
        /// <summary>
        /// Combines two given expressions by using the OR semantics.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="first">The first part of the expression.</param>
        /// <param name="second">The second part of the expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
    }
}
