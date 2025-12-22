using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SpecificationExample.Domain.Common;
using System.Linq.Expressions;


namespace SpecificationExample.Infra;

public static class SpecificationApplyExtensions
{
    public static IQueryable<T> FilterWithEfCore<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        return query.ApplySpecificationWithEFCore(specification);
    }

    private static IQueryable<T> ApplySpecificationWithEFCore<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        if (specification.QueryTags.Any())
        {
            //TODO: in EF Core Namespace
            foreach (var tag in specification.QueryTags)
            {
                query = query.TagWith(tag);
            }
        }

        if (specification.Includes is not null && specification.Includes.Count > 0)
        {
            for (int i = 0; i < specification.Includes.Count; i++)
            {
                Expression<Func<T, object>>? include = specification.Includes[i];
                if (i == 0)
                {
                    //TODO: in EF Core Namespace
                    query = query.Include(include);
                }
                else
                {
                    //TODO: in EF Core Namespace
                    query = ((IIncludableQueryable<T, object>)query).Include(include);
                }
            }
        }

        if (specification.AsNoTracking)
        {
            //TODO: in EF Core Namespace
            query = query.AsNoTracking();
        }

        if (specification.SplitQuery)
        {
            query = query.AsSplitQuery();
        }
    
        return query.Filter(specification);
    }  
}
