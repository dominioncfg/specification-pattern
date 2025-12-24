using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class DomainNotSpecification<T> : DomainSpecification<T> where T : Entity
{
    public DomainNotSpecification(DomainSpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(specification);

        var expr = specification.ToExpression();
        if (expr is null)
            ArgumentNullException.ThrowIfNull(expr);

        var notExpr = Expression.Lambda<Func<T, bool>>(
            Expression.Not(expr.Body),
            expr.Parameters);
       
        Rule(notExpr);
        OrderBy([.. specification.OrdersBy]);
        Paginate(specification.PagingExpressions);
    }
}
