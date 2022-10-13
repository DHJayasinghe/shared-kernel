using SharedKernel.Specification;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Interfaces.Repository;

/// <summary>
/// <para>
/// A <see cref="IRepositoryBase{T}" /> can be used to query and save instances of <typeparamref name="T" />.
/// An <see cref="Specification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
public interface IRepositoryBase<T> : IReadRepositoryBase<T> where T : class
{
    /// <summary>
    /// Adds an entity in the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="T" />.
    /// </returns>
    void Add(T entity);
    /// <summary>
    /// Updates an entity in the database
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    //void Update(T entity);

    /// <summary>
    /// Persists changes to the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Removes an entity in the database
    ///// </summary>
    ///// <param name="entity">The entity to delete.</param>
    ///// <returns>A task that represents the asynchronous operation.</returns>
    //void Delete(T entity);
    ///// <summary>
    ///// Removes the given entities in the database
    ///// </summary>
    ///// <param name="entities">The entities to remove.</param>
    ///// <returns>A task that represents the asynchronous operation.</returns>
    //void DeleteRange(IEnumerable<T> entities);
}