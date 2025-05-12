using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }
    public DbSet<ProviderService> ProviderServices { get; set; }
    public DbSet<ProviderServiceCountry> ProviderServiceCountries { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aquí le indico a EF que no genere tabla para Country
        modelBuilder.Ignore<Country>();

        modelBuilder.Entity<Provider>(b =>
        {
            b.ToTable("Providers");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedOnAdd();
            b.Property(x => x.Nit).IsRequired();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Email).IsRequired();

            b.HasMany(x => x.CustomFields)
                .WithOne(cf => cf.Provider)
                .HasForeignKey(cf => cf.ProviderId);

            b.HasMany(x => x.ProviderServices)
                .WithOne(ps => ps.Provider)
                .HasForeignKey(ps => ps.ProviderId);
        });

        modelBuilder.Entity<Service>(b =>
        {
            b.ToTable("Services");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedOnAdd();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.HourlyRate)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.HasMany(x => x.ProviderServices)
                .WithOne(ps => ps.Service)
                .HasForeignKey(ps => ps.ServiceId);
        });

        modelBuilder.Entity<ProviderService>(b =>
        {
            b.ToTable("ProviderServices");
            b.HasKey(x => x.ProviderServiceId);
            b.Property(x => x.ProviderServiceId).ValueGeneratedOnAdd();

            b.HasOne(ps => ps.Provider)
                .WithMany(p => p.ProviderServices)
                .HasForeignKey(ps => ps.ProviderId);

            b.HasOne(ps => ps.Service)
                .WithMany(s => s.ProviderServices)
                .HasForeignKey(ps => ps.ServiceId);

            b.HasMany(x => x.ServiceCountries)
                .WithOne(psc => psc.ProviderService)
                .HasForeignKey(psc => psc.ProviderServiceId);
        });

        modelBuilder.Entity<ProviderServiceCountry>(b =>
        {
            b.ToTable("ProviderServiceCountries");
            b.HasKey(x => new { x.ProviderServiceId, x.CountryIsoCode });

            b.HasOne(psc => psc.ProviderService)
                .WithMany(ps => ps.ServiceCountries)
                .HasForeignKey(psc => psc.ProviderServiceId);
        });

        modelBuilder.Entity<CustomField>(b =>
        {
            b.ToTable("CustomFields");
            b.HasKey(x => x.Id);
            b.Property(x => x.Key).IsRequired();
            b.Property(x => x.Value).IsRequired();
        });

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.Id);
            b.Property(x => x.Username).IsRequired();
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.Email).IsRequired();
        });
    }
}
