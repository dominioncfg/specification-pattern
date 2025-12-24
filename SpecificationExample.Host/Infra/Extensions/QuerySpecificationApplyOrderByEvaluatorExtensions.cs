using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Common;


namespace SpecificationExample.Infra;

public static class QuerySpecificationApplyOrderByEvaluatorExtensions
{
    public static IQueryable<T> ApplySpecificationOrderBy<T>(this IQueryable<T> query, QuerySpecification<T> specification) where T : Entity
    {
        if (specification is null)
            return query;


        var order = specification.OrdersBy;

        if (order is null || order.Count == 0)
            return query;


        var firstOrder = order[0];
        query = ApplyOrderBy(query, firstOrder);

        for (var i = 1; i < order.Count; i++)
        {
            var ord = order[i];
            query = ApplyThenBy(query, ord);
        }

        return query;
    }

    private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, QuerySpecificationOrderByExpression<T> order) where T : Entity
    {
        if (order.SortOrder == QuerySpecificationSortOrder.Ascending)
        {
            query = query.OrderBy(order.KeySelector);
        }
        else
        {
            query = query.OrderByDescending(order.KeySelector);
        }

        return query;
    }

    private static IQueryable<T> ApplyThenBy<T>(IQueryable<T> query, QuerySpecificationOrderByExpression<T> order) where T : Entity
    {
        if (order.SortOrder == QuerySpecificationSortOrder.Ascending)
        {
            query = ((IOrderedQueryable<T>)query).ThenBy(order.KeySelector);
        }
        else
        {
            query = ((IOrderedQueryable<T>)query).ThenByDescending(order.KeySelector);
        }

        return query;
    }
}
