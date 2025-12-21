using System.Linq.Expressions;

namespace SpecificationTests.Domain.Common;

public class OrSpecification<T> : Specification<T> where T : Entity
{
    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        var leftExpresssion = left.ToExpression();
        var rightExpression = right.ToExpression();


        if (leftExpresssion is null && rightExpression is null)
            throw new ArgumentException("Both specifications cannot be null");

        if (leftExpresssion is null)
        {
            Rule(rightExpression!);
            return;
        }

        if (rightExpression is null)
        {
            Rule(leftExpresssion!);
            return;
        }

        Rule(ExpressionCombiner.CombineExpressions(leftExpresssion, rightExpression, Expression.OrElse));
        OrderBy([.. left.OrdersBy, .. right.OrdersBy]);
        Paginate(left.PagingExpressions ?? right.PagingExpressions);
        Include([.. left.Includes, .. right.Includes]);
        QueryTag([.. left.QueryTags, .. right.QueryTags]);
    }
}
