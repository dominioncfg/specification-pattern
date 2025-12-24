namespace SpecificationExample.Domain.Common;

public interface IIncludableQuerySpecificationBuilder<T, out TProperty> where T : Entity
{
    QuerySpecification<T> Specification { get; }
}
