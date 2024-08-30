using ReservationSample.Model;

namespace ReservationSample.Services
{
    internal class RepositoryService : IRepositoryService<Reservation>
    {
        private readonly List<Reservation> repository = [];

        public Task AddAsync(Reservation entity)
        {
            if (repository.Any(item => item.Details.RoomNumber == entity.Details.RoomNumber))
            {
                throw new InvalidOperationException($"Room {entity.Details.RoomNumber} already reserved.");
            }

            repository.Add(entity);

            return Task.CompletedTask;
        }

        public Task<Reservation?> GetAsync(long id)
        {
            return Task.FromResult(repository.FirstOrDefault(item => item.Id == id));
        }

        public Task<int> GetCountAsync()
        {
            return Task.FromResult(repository.Count);
        }

        public Task UpdateAsync(long id, Reservation entity)
        {
            var reservation = repository.FirstOrDefault(item => item.Id == id) ?? throw new KeyNotFoundException();
            repository[repository.IndexOf(reservation)] = entity;

            return Task.CompletedTask;
        }
    }
}
