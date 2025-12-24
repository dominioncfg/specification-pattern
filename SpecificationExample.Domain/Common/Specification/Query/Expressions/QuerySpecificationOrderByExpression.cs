using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class QuerySpecificationOrderByExpression<T> where T : Entity
{
    public Expression<Func<T, object>> KeySelector { get; private set; }
    public QuerySpecificationSortOrder SortOrder { get; private set; }

    public QuerySpecificationOrderByExpression(Expression<Func<T, object>> keySelector, QuerySpecificationSortOrder sortOrder = QuerySpecificationSortOrder.Ascending)
    {
        KeySelector = keySelector;
        SortOrder = sortOrder;
    }
}

public enum QuerySpecificationSortOrder
{
    Ascending,
    Descending
}
