using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var infraDir = Directory.GetCurrentDirectory();
        var apiDir = Path.GetFullPath(Path.Combine(infraDir, "..", "API"));
            
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiDir)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connStr = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer((string?)connStr!, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());

        return new AppDbContext(optionsBuilder.Options);
    }
}