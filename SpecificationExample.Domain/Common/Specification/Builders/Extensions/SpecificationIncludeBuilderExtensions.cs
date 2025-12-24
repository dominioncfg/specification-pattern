using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public static class SpecificationIncludeBuilderExtensions
{
    public static IIncludableSpecificationBuilder<TEntity, EntityNavigation> Include<TEntity, TPreviousProperty, EntityNavigation>(this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> builder, Expression<Func<TEntity, EntityNavigation>> include)
        where TEntity : Entity
    {
        builder.Specification.Include(new IncludeExpressionInfo(include));
        var childBuilder = new IncludableSpecificationBuilder<TEntity, EntityNavigation>(builder.Specification);
        return childBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
       this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> builder,
       Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
       where TEntity : Entity
    {
        var expr = new IncludeExpressionInfo(navigationSelector, typeof(IEnumerable<TPreviousProperty>));
        builder.Specification.Include(expr);
        var childBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return childBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
       this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> builder,
       Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
       where TEntity : Entity
    {
        var expr = new IncludeExpressionInfo(navigationSelector, typeof(TPreviousProperty));
        builder.Specification.Include(expr);
        var childBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return childBuilder;
    }
}