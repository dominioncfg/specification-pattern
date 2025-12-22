using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class NotSpecification<T> : Specification<T> where T : Entity
{
    public NotSpecification(Specification<T> specification)
    {
        var expr = specification.ToExpression();
        if (expr is null)
            throw new ArgumentException("Specification cannot be null");

        var notExpr = Expression.Lambda<Func<T, bool>>(
            Expression.Not(expr.Body),
            expr.Parameters);
        Rule(notExpr);
        OrderBy(specification.OrdersBy.ToArray());
        Paginate(specification.PagingExpressions);
        Include([.. specification.Includes]);
        QueryTag([.. specification.QueryTags]);
    }
}
