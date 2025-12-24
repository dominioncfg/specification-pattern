using System.Linq.Expressions;

namespace SpecificationExample.Domain.Common;

public static class QuerySpecificationIncludeBuilderExtensions
{
    public static IIncludableQuerySpecificationBuilder<TEntity, EntityNavigation> Include<TEntity, TPreviousProperty, EntityNavigation>(this IIncludableQuerySpecificationBuilder<TEntity, TPreviousProperty> builder, Expression<Func<TEntity, EntityNavigation>> include)
        where TEntity : Entity
    {
        builder.Specification.Include(new QuerySpecificationIncludeExpressionInfo(include));
        var childBuilder = new IncludableQuerySpecificationBuilder<TEntity, EntityNavigation>(builder.Specification);
        return childBuilder;
    }

    public static IIncludableQuerySpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
       this IIncludableQuerySpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> builder,
       Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
       where TEntity : Entity
    {
        var expr = new QuerySpecificationIncludeExpressionInfo(navigationSelector, typeof(IEnumerable<TPreviousProperty>));
        builder.Specification.Include(expr);
        var childBuilder = new IncludableQuerySpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return childBuilder;
    }

    public static IIncludableQuerySpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
       this IIncludableQuerySpecificationBuilder<TEntity, TPreviousProperty> builder,
       Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
       where TEntity : Entity
    {
        var expr = new QuerySpecificationIncludeExpressionInfo(navigationSelector, typeof(TPreviousProperty));
        builder.Specification.Include(expr);
        var childBuilder = new IncludableQuerySpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return childBuilder;
    }
}