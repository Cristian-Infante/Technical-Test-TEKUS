using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository(AppDbContext ctx) : Repository<Service>(ctx), IServiceRepository
{
    public async Task<Service?> GetWithProvidersAsync(Guid id) =>
        await Context.Services
            .Include(s => s.ProviderServices)
            .ThenInclude(ps => ps.Provider)
            .FirstOrDefaultAsync(s => s.Id == id);

    public Task<Service?> GetByIdWithProvidersAsync(Guid id)
    {
        return Context.Services
            .Include(s => s.ProviderServices)
            .ThenInclude(ps => ps.Provider)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}