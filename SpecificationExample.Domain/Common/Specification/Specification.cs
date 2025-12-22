using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class Specification<T> where T : Entity
{
    public Expression<Func<T, bool>>? Filter { get; private set; }
    public List<SpecificationOrderByExpression<T>> OrdersBy { get; protected set; } = [];
    public SpecificationPagingExpression<T>? PagingExpressions { get; protected set; }
    public List<Expression<Func<T, object>>> Includes { get; protected set; } = [];
    public List<string> QueryTags { get; protected set; } = [];
    public bool SplitQuery { get; protected set; } = false;
    public bool AsNoTracking { get; protected set; } = false;


    public Specification<T> Rule(Expression<Func<T, bool>> filter)
    {
        Filter = filter;
        return this;
    }

    public Expression<Func<T, bool>>? ToExpression() => Filter;

    public Specification<T> OrderBy(Expression<Func<T, object>> orderBy)
    {
        OrdersBy.Add(new SpecificationOrderByExpression<T>(orderBy));
        return this;
    }

    public Specification<T> OrderByDescending(Expression<Func<T, object>> orderBy)
    {
        OrdersBy.Add(new SpecificationOrderByExpression<T>(orderBy, SpecificationSortOrder.Descending));
        return this;
    }
    protected Specification<T> OrderBy(params SpecificationOrderByExpression<T>[] orderBy)
    {
        OrdersBy.AddRange(orderBy);
        return this;
    }
    public Specification<T> Paginate(int skip, int take)
    {
        PagingExpressions = new SpecificationPagingExpression<T>(skip, take);
        return this;
    }
    public Specification<T> Paginate(SpecificationPagingExpression<T>? pagingExpression)
    {
        PagingExpressions = pagingExpression;
        return this;
    }

    public Specification<T> Include(Expression<Func<T, object>> include)
    {
        Includes.Add(include);
        return this;
    }

    protected Specification<T> Include(params Expression<Func<T, object>>[] includes)
    {
        Includes.AddRange(includes);
        return this;
    }

    public Specification<T> QueryTag(string tag)
    {
        QueryTags.Add(tag);
        return this;
    }

    protected Specification<T> QueryTag(params string[] tags)
    {
        QueryTags.AddRange(tags);
        return this;
    }

    public Specification<T> AsNoTrackingQuery(bool asNoTracking = true)
    {
        AsNoTracking = asNoTracking;
        return this;
    }

    public Specification<T> AsSplitQuery(bool asSplitQuery = true)
    {
        SplitQuery = asSplitQuery;
        return this;
    }


    public Specification<T> And(Specification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }

    public Specification<T> Or(Specification<T> other)
    {
        return new OrSpecification<T>(this, other);
    }

    public Specification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}
