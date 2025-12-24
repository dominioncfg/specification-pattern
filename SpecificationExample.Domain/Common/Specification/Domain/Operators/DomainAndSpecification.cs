using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class DomainAndSpecification<T> : DomainSpecification<T> where T : Entity
{
    public DomainAndSpecification(DomainSpecification<T> left, DomainSpecification<T> right)
    {
        if (left is null && right is null)
            throw new ArgumentException("Both specifications cannot be null");

        Rule(CombineRule(left, right));
        OrderBy(CombineOrderBy(left, right));
        Paginate(CombinePaging(left, right));
    }

    private static Expression<Func<T, bool>>? CombineRule(DomainSpecification<T>? left, DomainSpecification<T>? right)
    {
        var leftExpresssion = left?.ToExpression();
        var rightExpression = right?.ToExpression();

        if (leftExpresssion is null)
            return rightExpression;

        if (rightExpression is null)
            return leftExpresssion;

        return ExpressionCombiner.CombineExpressions(leftExpresssion, rightExpression, Expression.AndAlso);
    }

    private static DomainSpecificationOrderByExpression<T>[] CombineOrderBy(DomainSpecification<T>? left, DomainSpecification<T>? right)
    {
        return [.. left?.OrdersBy ?? [], .. right?.OrdersBy ?? []];
    }

    private static DomainSpecificationPagingExpression<T>? CombinePaging(DomainSpecification<T>? left, DomainSpecification<T>? right)
    {
        return left?.PagingExpressions ?? right?.PagingExpressions;
    }
}
