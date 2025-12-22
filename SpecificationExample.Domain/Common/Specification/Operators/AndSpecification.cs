using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class AndSpecification<T> : Specification<T> where T : Entity
{
    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        if (left is null && right is null)
            throw new ArgumentException("Both specifications cannot be null");

        Rule(CombineRule(left, right));
        OrderBy(CombineOrderBy(left, right));
        Paginate(CombinePaging(left, right));
        Include(CombineIncludes(left, right));
        QueryTag(CombineTags(left, right));
        AsNoTrackingQuery(CombineAsNoTracking(left, right));
        AsSplitQuery(CombineSplitQuery(left, right));
    }

    private static Expression<Func<T, bool>>? CombineRule(Specification<T>? left, Specification<T>? right)
    {
        var leftExpresssion = left?.ToExpression();
        var rightExpression = right?.ToExpression();

        if (leftExpresssion is null)
            return rightExpression;

        if (rightExpression is null)
            return leftExpresssion;

        return ExpressionCombiner.CombineExpressions(leftExpresssion, rightExpression, Expression.AndAlso);
    }

    private static SpecificationOrderByExpression<T>[] CombineOrderBy(Specification<T>? left, Specification<T>? right)
    {
        return [.. left?.OrdersBy ?? [], .. right?.OrdersBy ?? []];
    }

    private static SpecificationPagingExpression<T>? CombinePaging(Specification<T>? left, Specification<T>? right)
    {
        return left?.PagingExpressions ?? right?.PagingExpressions;
    }

    private static Expression<Func<T, object>>[] CombineIncludes(Specification<T>? left, Specification<T>? right)
    {
        return [.. left?.Includes ?? [], .. right?.Includes ?? []];
    }

    private static string[] CombineTags(Specification<T>? left, Specification<T>? right)
    {
        return [.. left?.QueryTags ?? [], .. right?.QueryTags ?? []];
    }

    private static bool? CombineAsNoTracking(Specification<T>? left, Specification<T>? right)
    {
        return left?.AsNoTracking ?? right?.AsNoTracking;
    }

    private static bool? CombineSplitQuery(Specification<T>? left, Specification<T>? right)
    {
        return left?.SplitQuery ?? right?.SplitQuery;
    }
}
