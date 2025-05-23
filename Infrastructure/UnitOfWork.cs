﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure;

public class UnitOfWork(
    AppDbContext context,
    IUserRepository userRepo,
    IRepository<Provider> providers,
    IRepository<Service> services,
    IRepository<CustomField> customFields,
    IRepository<ProviderService> providerServices,
    IRepository<ProviderServiceCountry> providerServiceCountries
) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    public IUserRepository Users { get; } = userRepo;
    public IRepository<Provider> Providers { get; } = providers;
    public IRepository<Service> Services { get; } = services;
    public IRepository<CustomField> CustomFields { get; } = customFields;
    public IRepository<ProviderService> ProviderServices { get; } = providerServices;
    public IRepository<ProviderServiceCountry> ProviderServiceCountries { get; } = providerServiceCountries;
    
    public int Save() => context.SaveChanges();

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;
        _currentTransaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("There is no active transaction.");

        await context.SaveChangesAsync();
        await _currentTransaction.CommitAsync();
        await DisposeTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.RollbackAsync();
        await DisposeTransactionAsync();
    }

    public async Task DisposeTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            context.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
