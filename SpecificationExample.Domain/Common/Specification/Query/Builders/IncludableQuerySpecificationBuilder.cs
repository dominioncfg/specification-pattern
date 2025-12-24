namespace SpecificationExample.Domain.Common;

public class IncludableQuerySpecificationBuilder<TEntity, TProperty>: IIncludableQuerySpecificationBuilder<TEntity,TProperty> where TEntity : Entity
{
    private readonly QuerySpecification<TEntity> _specification;
    public QuerySpecification<TEntity> Specification => _specification;

    public IncludableQuerySpecificationBuilder(QuerySpecification<TEntity> specification)
    {
        _specification = specification;
    }
}
