namespace SpecificationTests.Domain.Common;

public static class SpecificationApplyExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        return query.ApplySpecification(specification);
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> query, Specification<T> specification) where T : Entity
    {
        return query.ApplySpecification(specification);
    }

    private static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, Specification<T> specification) where T : Entity
    {
        //if (specification.QueryTags.Any())
        //{
        //    //TODO: in EF Core Namespace
        //    foreach (var tag in specification.QueryTags)
        //    {
        //        query = query.TagWith(tag);
        //    }
        //}

        var filter = specification.ToExpression();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var order = specification.OrdersBy;
        if (order is not null && order.Count > 0)
        {
            for (var i = 0; i < order.Count; i++)
            {
                var ord = order[i];
                if (i == 0)
                {
                    if (ord.SortOrder == SpecificationSortOrder.Ascending)
                    {
                        query = query.OrderBy(ord.KeySelector);
                    }
                    else
                    {
                        query = query.OrderByDescending(ord.KeySelector);
                    }
                }
                else
                {
                    if (ord.SortOrder == SpecificationSortOrder.Ascending)
                    {
                        query = ((IOrderedQueryable<T>)query).ThenBy(ord.KeySelector);
                    }
                    else
                    {
                        query = ((IOrderedQueryable<T>)query).ThenByDescending(ord.KeySelector);
                    }
                }
            }
        }

        if (specification.PagingExpressions is not null)
        {
            var paging = specification.PagingExpressions;
            if (paging.Skip > 0)
            {
                query = query.Skip(paging.Skip);
            }
            if (paging.Take != int.MaxValue)
            {
                query = query.Take(paging.Take);
            }
        }

        //if (specification.Includes is not null && specification.Includes.Count > 0)
        //{
        //    for (int i = 0; i < specification.Includes.Count; i++)
        //    {
        //        Expression<Func<T, object>>? include = specification.Includes[i];
        //        if (i == 0)
        //        {
        //            //TODO: in EF Core Namespace
        //            query = query.Include(include);
        //        }
        //        else
        //        {
        //            //TODO: in EF Core Namespace
        //            query = ((IIncludableQueryable<T, object>)query).Include(include);
        //        }
        //    }
        //}

        //if (specification.AsNoTracking)
        //{
        //    //TODO: in EF Core Namespace
        //    query = query.AsNoTracking();
        //}

        //if (specification.SplitQuery)
        //{
        //    query = query.AsSplitQuery();
        //}

        return query;
    }

    private static IEnumerable<T> ApplySpecification<T>(this IEnumerable<T> query, Specification<T> specification) where T : Entity
    {
        return query.AsQueryable().ApplySpecification(specification).AsEnumerable();
    }
}
