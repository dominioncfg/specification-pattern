namespace SpecificationExample.Domain.Common;

public static class DomainSpecificationApplyExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, DomainSpecification<T> specification) where T : Entity
    {
        return query.ApplySpecification(specification);
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> query, DomainSpecification<T> specification) where T : Entity
    {
        return query.AsQueryable().ApplySpecification(specification).AsEnumerable();
    }

    public static bool IsSatisfiedBy<T>(this DomainSpecification<T> specification, T entity) where T : Entity
    {
        ArgumentNullException.ThrowIfNull(specification);
        ArgumentNullException.ThrowIfNull(entity);
        
        var predicate = specification.ToExpression();
        if (predicate is null)
            return true;
        
        return predicate.Compile().Invoke(entity);
    }

    public static bool IsNotSatisfiedBy<T>(this DomainSpecification<T> specification, T entity) where T : Entity
    {
        return !specification.IsSatisfiedBy(entity);
    }

    public static bool IsSatisfiedBy<T>(this T entity, DomainSpecification<T> specification) where T : Entity
    {
        return specification.IsSatisfiedBy(entity);
    }
    public static bool IsNotSatisfiedBy<T>(this T entity, DomainSpecification<T> specification) where T : Entity
    {
        return !specification.IsSatisfiedBy(entity);
    }

    private static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, DomainSpecification<T> specification) where T : Entity
    {
        return query
            .ApplySpecificationFilter(specification)
            .ApplySpecificationOrderBy( specification)
            .ApplyPagingToSpecification(specification);
    }

    private static IQueryable<T> ApplySpecificationFilter<T>(this IQueryable<T> query, DomainSpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        var filter = specification.ToExpression();
        if (filter is null)
            return query;

        return query.Where(filter);
    }

    private static IQueryable<T> ApplySpecificationOrderBy<T>(this IQueryable<T> query, DomainSpecification<T> specification) where T : Entity
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

    private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, DomainSpecificationOrderByExpression<T> order) where T : Entity
    {
        if (order.SortOrder == SpecificationSortOrder.Ascending)
        {
            query = query.OrderBy(order.KeySelector);
        }
        else
        {
            query = query.OrderByDescending(order.KeySelector);
        }

        return query;
    }

    private static IQueryable<T> ApplyThenBy<T>(IQueryable<T> query, DomainSpecificationOrderByExpression<T> order) where T : Entity
    {
        if (order.SortOrder == SpecificationSortOrder.Ascending)
        {
            query = ((IOrderedQueryable<T>)query).ThenBy(order.KeySelector);
        }
        else
        {
            query = ((IOrderedQueryable<T>)query).ThenByDescending(order.KeySelector);
        }

        return query;
    }

    private static IQueryable<T> ApplyPagingToSpecification<T>(this IQueryable<T> query, DomainSpecification<T> specification) where T : Entity
    {
        if (specification is null)
            return query;

        if (specification.PagingExpressions is null)
            return query;

        var paging = specification.PagingExpressions;
        if (paging.Skip > 0)
            query = query.Skip(paging.Skip);

        if (paging.Take >= 0 && paging.Take != int.MaxValue)
            query = query.Take(paging.Take);

        return query;
    }
}
