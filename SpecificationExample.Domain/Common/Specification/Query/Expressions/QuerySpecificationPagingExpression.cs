namespace SpecificationExample.Domain.Common;

public class QuerySpecificationPagingExpression<T> where T : Entity
{
    public int Take { get; private set; }

    public int Skip { get; private set; }

    public QuerySpecificationPagingExpression(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}
