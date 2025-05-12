using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProviderRepository(AppDbContext ctx) : Repository<Provider>(ctx), IProviderRepository
{
    public Task<Provider?> GetByIdWithServicesAsync(Guid id) =>
        Context.Providers
            .Include(p => p.CustomFields)
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.Service)
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.ServiceCountries)
            .SingleOrDefaultAsync(p => p.Id == id);

    public Task<IEnumerable<Provider>> GetAllWithServicesAsync() =>
        Context.Providers
            .Include(p => p.CustomFields)
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.Service)
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.ServiceCountries)
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<Provider>)t.Result);
}