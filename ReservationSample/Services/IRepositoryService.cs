namespace ReservationSample.Services
{
    /// <summary>
    /// Defines a repository service for managing entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public interface IRepositoryService<T>
    {
        /// <summary>
        /// Retrieves an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains the entity of type <typeparamref name="T"/> if found; otherwise, <see langword="null"/>.
        /// </returns>
        Task<T?> GetAsync(long id);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to add an entity with a duplicate identifier.
        /// </exception>
        /// <returns>A <see cref="Task"/>  that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity identified by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to update.</param>
        /// <param name="entity">The new entity data to update the existing entity.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the entity of the specified identifier is not found in the repository.
        /// </exception>
        /// <returns></returns>
        Task UpdateAsync(long id, T entity);

        /// <summary>
        /// Retrieves the total count of entities asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
        /// The task result contains the count of entities.
        /// </returns>
        Task<int> GetCountAsync();
    }
}
