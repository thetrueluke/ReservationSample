namespace ReservationSample.Services
{
    public interface IRepositoryService<T>
    {
        Task<T?> GetAsync(long id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
