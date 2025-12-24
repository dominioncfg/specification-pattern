namespace SpecificationExample.Domain.Common;

public interface IRepository<T> where T : Entity
{
    Task<IEnumerable<T>> Filter(QuerySpecification<T> filter, CancellationToken cancellation);
    Task<T> Get(QuerySpecification<T> filter, CancellationToken cancellation);
    Task<T?> GetById(int id, CancellationToken cancellationToken);
    Task Add(T entity, CancellationToken cancellationToken);
    Task Delete(T entity, CancellationToken cancellation);
}
