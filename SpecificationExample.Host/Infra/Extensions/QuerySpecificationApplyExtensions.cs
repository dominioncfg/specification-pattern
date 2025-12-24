using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Common;


namespace SpecificationExample.Infra;

public static class QuerySpecificationApplyExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, QuerySpecification<T> specification) where T : Entity
    {
        return query.ApplySpecificationWithEFCore(specification);
    }

    private static IQueryable<T> ApplySpecificationWithEFCore<T>(this IQueryable<T> query, QuerySpecification<T> specification) where T : Entity
    {
        return query
             .ApplyQueryTags(specification)
             .ApplyIncludes(specification)
             .ApplyAsNoTracking(specification)
             .ApplyAsSplitQuery(specification)
             .ApplySpecificationFilter(specification)
             .ApplySpecificationOrderBy(specification)
             .ApplyPagingToSpecification(specification);
    }

    private static IQueryable<T> ApplySpecificationFilter<T>(this IQueryable<T> query, QuerySpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        var filter = specification.FilterRule;
        if (filter is null)
            return query;

        return query.Where(filter);
    }

    private static IQueryable<T> ApplyPagingToSpecification<T>(this IQueryable<T> query, QuerySpecification<T> specification) where T : Entity
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

    private static IQueryable<T> ApplyQueryTags<T>(this IQueryable<T> query, QuerySpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        if (specification.QueryTags is null || !specification.QueryTags.Any())
            return query;

        foreach (var tag in specification.QueryTags)
        {
            query = query.TagWith(tag);
        }

        return query;
    }

    private static IQueryable<T> ApplyAsNoTracking<T>(this IQueryable<T> query, QuerySpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        if (!specification.AsNoTracking.HasValue)
            return query;

        if (specification.AsNoTracking.Value)
        {
            query = query.AsNoTracking();
        }

        if (!specification.AsNoTracking.Value)
        {
            query = query.AsTracking();
        }

        return query;
    }

    private static IQueryable<T> ApplyAsSplitQuery<T>(this IQueryable<T> query, QuerySpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        if (!specification.SplitQuery.HasValue)
            return query;

        if (specification.SplitQuery.Value)
        {
            query = query.AsSplitQuery();
        }

        return query;
    }
}
