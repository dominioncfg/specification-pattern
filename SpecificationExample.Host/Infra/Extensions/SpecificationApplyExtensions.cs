using Microsoft.EntityFrameworkCore;
using SpecificationExample.Domain.Common;


namespace SpecificationExample.Infra;

public static class SpecificationApplyExtensions
{
    public static IQueryable<T> FilterWithEfCore<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        return query.ApplySpecificationWithEFCore(specification);
    }

    private static IQueryable<T> ApplySpecificationWithEFCore<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        return query
             .ApplyQueryTags(specification)
             .ApplyIncludes(specification)
             .ApplyAsNoTracking(specification)
             .ApplyAsSplitQuery(specification)
             .Filter(specification);
    }

    private static IQueryable<T> ApplyQueryTags<T>(this IQueryable<T> query, Specification<T>? specification) where T : Entity
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

    private static IQueryable<T> ApplyAsNoTracking<T>(this IQueryable<T> query, Specification<T>? specification) where T : Entity
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

    private static IQueryable<T> ApplyAsSplitQuery<T>(this IQueryable<T> query, Specification<T>? specification) where T : Entity
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
