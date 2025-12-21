namespace SpecificationTests.Domain.Common;

public class SpecificationPagingExpression<T> where T : Entity
{
    public int Take { get; private set; }

    public int Skip { get; private set; }

    public SpecificationPagingExpression(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }
}
