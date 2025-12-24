namespace SpecificationExample.Domain.Common;

public class DomainSpecificationPagingExpression<T> where T : Entity
{
    public int Take { get; private set; }

    public int Skip { get; private set; }

    public DomainSpecificationPagingExpression(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}
