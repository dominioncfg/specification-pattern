namespace SpecificationExample.Domain.Common;

public interface IIncludableSpecificationBuilder<T, out TProperty> where T : Entity
{
    Specification<T> Specification { get; }
}
