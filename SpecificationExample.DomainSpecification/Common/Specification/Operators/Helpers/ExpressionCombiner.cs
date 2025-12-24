using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

internal static class ExpressionCombiner
{
    public static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, BinaryExpression> combiner) where T : Entity
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var leftVisitor = new ReplaceParameterVisitor(left.Parameters[0], parameter);
        var leftBody = leftVisitor.Visit(left.Body);
        var rightVisitor = new ReplaceParameterVisitor(right.Parameters[0], parameter);
        var rightBody = rightVisitor.Visit(right.Body);
        var combined = combiner(leftBody!, rightBody!);
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    private class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
