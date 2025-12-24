using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class QuerySpecification<T> where T : Entity
{
    private Expression<Func<T, bool>>? _filter;
    private List<QuerySpecificationOrderByExpression<T>> _ordersBy = [];
    private QuerySpecificationPagingExpression<T>? _pagingExpressions;
    private List<QuerySpecificationIncludeExpressionInfo> _includes = [];
    private List<string> _queryTags = [];
    private bool? _splitQuery;
    private bool? _asNoTracking;
    
    public Expression<Func<T, bool>>? FilterRule => _filter;
    public IReadOnlyList<QuerySpecificationOrderByExpression<T>> OrdersBy => _ordersBy;
    public QuerySpecificationPagingExpression<T>? PagingExpressions => _pagingExpressions;
    public IReadOnlyList<QuerySpecificationIncludeExpressionInfo> Includes => _includes;
    public IReadOnlyList<string> QueryTags => _queryTags;
    public bool? SplitQuery => _splitQuery;
    public bool? AsNoTracking => _asNoTracking;

    public QuerySpecification<T> Rule(Expression<Func<T, bool>>? filter)
    {
        _filter = filter;
        return this;
    }

    public QuerySpecification<T> OrderBy(Expression<Func<T, object>> orderBy)
    {
        _ordersBy.Add(new QuerySpecificationOrderByExpression<T>(orderBy));
        return this;
    }

    public QuerySpecification<T> OrderByDescending(Expression<Func<T, object>> orderBy)
    {
        _ordersBy.Add(new QuerySpecificationOrderByExpression<T>(orderBy, QuerySpecificationSortOrder.Descending));
        return this;
    }
    
    public QuerySpecification<T> Paginate(int skip, int take)
    {
        _pagingExpressions = new QuerySpecificationPagingExpression<T>(skip, take);
        return this;
    }

    public QuerySpecification<T> Include(QuerySpecificationIncludeExpressionInfo include)
    {
        _includes.Add(include);
        return this;
    }

    public IncludableQuerySpecificationBuilder<T, EntityNavigation> Include<EntityNavigation>(Expression<Func<T, EntityNavigation>> include)
    {
        Include(new QuerySpecificationIncludeExpressionInfo(include));
        var builder = new IncludableQuerySpecificationBuilder<T, EntityNavigation>(this);
        return builder;
    }

    public QuerySpecification<T> QueryTag(params string[] tags)
    {
        _queryTags.AddRange(tags);
        return this;
    }

    public QuerySpecification<T> AsNoTrackingQuery(bool? asNoTracking = true)
    {
        _asNoTracking = asNoTracking;
        return this;
    }

    public QuerySpecification<T> AsSplitQuery(bool? asSplitQuery = true)
    {
        _splitQuery = asSplitQuery;
        return this;
    }
}
