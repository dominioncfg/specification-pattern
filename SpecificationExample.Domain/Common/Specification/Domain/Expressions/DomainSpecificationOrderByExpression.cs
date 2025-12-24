using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class DomainSpecificationOrderByExpression<T> where T : Entity
{
    public Expression<Func<T, object>> KeySelector { get; private set; }
    public SpecificationSortOrder SortOrder { get; private set; }

    public DomainSpecificationOrderByExpression(Expression<Func<T, object>> keySelector, SpecificationSortOrder sortOrder = SpecificationSortOrder.Ascending)
    {
        KeySelector = keySelector;
        SortOrder = sortOrder;
    }
}

public enum SpecificationSortOrder
{
    Ascending,
    Descending
}
