namespace SpecificationExample.Domain.Common;

public class IncludableSpecificationBuilder<TEntity, TProperty>: IIncludableSpecificationBuilder<TEntity,TProperty> where TEntity : Entity
{
    private readonly Specification<TEntity> _specification;
    public Specification<TEntity> Specification => _specification;

    public IncludableSpecificationBuilder(Specification<TEntity> specification)
    {
        _specification = specification;
    }
}
