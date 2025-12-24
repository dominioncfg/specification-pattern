using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class QuerySpecificationIncludeExpressionInfo
{
    public LambdaExpression LambdaExpression { get; }

    public Type? PreviousPropertyType { get; }

    public QuerySpecificationIncludeTypeEnum Type { get; }

    public QuerySpecificationIncludeExpressionInfo(LambdaExpression expression)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        LambdaExpression = expression;
        PreviousPropertyType = null;
        Type = QuerySpecificationIncludeTypeEnum.Include;
    }

    public QuerySpecificationIncludeExpressionInfo(LambdaExpression expression, Type previousPropertyType)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));
        _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));

        LambdaExpression = expression;
        PreviousPropertyType = previousPropertyType;
        Type = QuerySpecificationIncludeTypeEnum.ThenInclude;
    }
}

public enum QuerySpecificationIncludeTypeEnum
{
    Include = 1,
    ThenInclude = 2
}
