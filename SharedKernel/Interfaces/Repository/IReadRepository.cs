namespace SharedKernel.Interfaces.Repository;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
