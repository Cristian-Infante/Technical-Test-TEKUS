using Domain.Entities;

namespace Domain.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    Task<Service?> GetByIdWithProvidersAsync(Guid id);
}