using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SpecificationExample.Domain.Common;
using System.Linq.Expressions;
using System.Reflection;


namespace SpecificationExample.Infra;

public static class SpecificationApplyIncludeEvaluatorExtensions
{
    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
       .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
       .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 2
           && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
           && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateIncludeDelegate(Type entityType, Type propertyType, Type? previousReturnType)
    {
        var includeMethod = _includeMethodInfo.MakeGenericMethod(entityType, propertyType);
        var sourceParameter = Expression.Parameter(typeof(IQueryable));
        var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

        var call = Expression.Call(
              includeMethod,
              Expression.Convert(sourceParameter, typeof(IQueryable<>).MakeGenericType(entityType)),
              Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, propertyType))));

        var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateThenIncludeDelegate(Type entityType, Type propertyType, Type previousReturnType)
    {

        var thenIncludeInfo = IsGenericEnumerable(previousReturnType, out var previousPropertyType)
            ? _thenIncludeAfterEnumerableMethodInfo
            : _thenIncludeAfterReferenceMethodInfo;

        var thenIncludeMethod = thenIncludeInfo.MakeGenericMethod(entityType, previousPropertyType, propertyType);
        var sourceParameter = Expression.Parameter(typeof(IQueryable));
        var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

        var call = Expression.Call(
                thenIncludeMethod,
                Expression.Convert(sourceParameter, typeof(IIncludableQueryable<,>).MakeGenericType(entityType, previousReturnType)),
                Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(previousPropertyType, propertyType))));

        var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static bool IsGenericEnumerable(Type type, out Type propertyType)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            propertyType = type.GenericTypeArguments[0];
            return true;
        }

        propertyType = type;
        return false;
    }
    public static IQueryable<T> ApplyIncludes<T>(this IQueryable<T> query, QuerySpecification<T>? specification) where T : Entity
    {
        if (specification is null)
            return query;

        if (specification.Includes is null || specification.Includes.Count == 0)
            return query;

        var currentQuery = query;

        for (int i = 0; i < specification.Includes.Count; i++)
        {
            var includeInfo = specification.Includes[i];

            if (includeInfo.Type == QuerySpecificationIncludeTypeEnum.Include)
            {
                var inculde = CreateIncludeDelegate(typeof(T), includeInfo.LambdaExpression.ReturnType, null);
                currentQuery = (IQueryable<T>)inculde(currentQuery, includeInfo.LambdaExpression);
            }
            else if (includeInfo.Type == QuerySpecificationIncludeTypeEnum.ThenInclude)
            {
                var inculde = CreateThenIncludeDelegate(typeof(T), includeInfo.LambdaExpression.ReturnType, includeInfo.PreviousPropertyType!);
                currentQuery = (IQueryable<T>)inculde(currentQuery, includeInfo.LambdaExpression);
            }
        }

        return currentQuery;
    }
}
