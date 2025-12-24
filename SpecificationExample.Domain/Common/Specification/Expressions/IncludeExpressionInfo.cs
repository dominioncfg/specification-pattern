using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public class IncludeExpressionInfo
{
    public LambdaExpression LambdaExpression { get; }

    public Type? PreviousPropertyType { get; }

    public IncludeTypeEnum Type { get; }

    public IncludeExpressionInfo(LambdaExpression expression)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        LambdaExpression = expression;
        PreviousPropertyType = null;
        Type = IncludeTypeEnum.Include;
    }

    public IncludeExpressionInfo(LambdaExpression expression, Type previousPropertyType)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));
        _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));

        LambdaExpression = expression;
        PreviousPropertyType = previousPropertyType;
        Type = IncludeTypeEnum.ThenInclude;
    }
}

public enum IncludeTypeEnum
{
    Include = 1,
    ThenInclude = 2
}
