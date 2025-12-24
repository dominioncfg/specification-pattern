using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class DomainSpecification<T> where T : Entity
{
    public Expression<Func<T, bool>>? Filter { get; private set; }
    public List<DomainSpecificationOrderByExpression<T>> OrdersBy { get; private set; } = [];
    public DomainSpecificationPagingExpression<T>? PagingExpressions { get; private set; }


    protected DomainSpecification<T> OrderBy(params DomainSpecificationOrderByExpression<T>[] orderBy)
    {
        OrdersBy.AddRange(orderBy);
        return this;
    }

    protected DomainSpecification<T> Paginate(DomainSpecificationPagingExpression<T>? pagingExpression)
    {
        PagingExpressions = pagingExpression;
        return this;
    }

    public DomainSpecification<T> Rule(Expression<Func<T, bool>>? filter)
    {
        Filter = filter;
        return this;
    }

    public Expression<Func<T, bool>>? ToExpression() => Filter;
    
    public DomainSpecification<T> OrderBy(Expression<Func<T, object>> orderBy)
    {
        OrdersBy.Add(new DomainSpecificationOrderByExpression<T>(orderBy));
        return this;
    }

    public DomainSpecification<T> OrderByDescending(Expression<Func<T, object>> orderBy)
    {
        OrdersBy.Add(new DomainSpecificationOrderByExpression<T>(orderBy, SpecificationSortOrder.Descending));
        return this;
    }
    
    public DomainSpecification<T> Paginate(int skip, int take)
    {
        PagingExpressions = new DomainSpecificationPagingExpression<T>(skip, take);
        return this;
    }
   

    public DomainSpecification<T> And(DomainSpecification<T> other)
    {
        return new DomainAndSpecification<T>(this, other);
    }

    public DomainSpecification<T> Or(DomainSpecification<T> other)
    {
        return new DomainOrSpecification<T>(this, other);
    }

    public DomainSpecification<T> Not()
    {
        return new DomainNotSpecification<T>(this);
    }
}
