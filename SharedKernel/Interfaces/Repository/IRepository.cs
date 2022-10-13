namespace SharedKernel.Interfaces.Repository;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
