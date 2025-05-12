using Domain.Entities;

namespace Domain.Interfaces;

public interface IProviderRepository : IRepository<Provider>
{
    Task<Provider?> GetByIdWithServicesAsync(Guid id);
    Task<IEnumerable<Provider>> GetAllWithServicesAsync();
}