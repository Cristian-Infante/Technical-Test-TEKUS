using Domain.Entities;

namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository Users { get; }
    public IRepository<Provider> Providers { get; }
    public IRepository<Service> Services { get; }
    public IRepository<Country> Countries { get; }
    public IRepository<CustomField> CustomFields { get; }
    public IRepository<ProviderService> ProviderServices { get; }
    public IRepository<ProviderServiceCountry> ProviderServiceCountries { get; }
    
    int Save();
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task DisposeTransactionAsync();
}