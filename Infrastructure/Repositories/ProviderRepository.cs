using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProviderRepository(AppDbContext ctx) : Repository<Provider>(ctx), IProviderRepository
{
    public Task<Provider?> GetByIdWithServicesAsync(Guid id) =>
        Context.Set<Provider>()
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.Service)
            .Include(p => p.CustomFields)
            .SingleOrDefaultAsync(p => p.Id == id);

    public Task<IEnumerable<Provider>> GetAllWithServicesAsync() =>
        Context.Set<Provider>()
            .Include(p => p.ProviderServices)
            .ThenInclude(ps => ps.Service)
            .Include(p => p.CustomFields)
            .ToListAsync()
            .ContinueWith(IEnumerable<Provider> (t) => t.Result);
}