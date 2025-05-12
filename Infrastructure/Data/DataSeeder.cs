using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DataSeeder
{
    private static readonly string[] CountryCodes =
    [
        "US","CA","MX","FR","DE","JP","CN","BR","IN","RU"
    ];

    public static async Task SeedAsync(AppDbContext ctx)
    {
        var faker = new Faker();

        if (!await ctx.Services.AnyAsync())
        {
            var svcFaker = new Faker<Service>()
                .RuleFor(s => s.Id, _ => Guid.NewGuid())
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.HourlyRate, f => f.Random.Decimal(20, 200));

            var services = svcFaker.Generate(10);
            await ctx.Services.AddRangeAsync(services);
            await ctx.SaveChangesAsync();
        }

        if (!await ctx.Providers.AnyAsync())
        {
            var services = await ctx.Services.ToListAsync();

            var provFaker = new Faker<Provider>()
                .CustomInstantiator(f => Provider.Create(
                    nit:   f.Company.Cnpj(),
                    name:  f.Company.CompanyName(),
                    email: f.Internet.Email()
                ));

            var providers = provFaker.Generate(10);

            foreach (var prov in providers)
            {
                var offered = faker.Random.ListItems(services, faker.Random.Int(1, 3));
                foreach (var svc in offered)
                {
                    var isoList = faker.Random.ListItems(CountryCodes, faker.Random.Int(1, 4));
                    prov.OfferService(svc, isoList);
                }

                for (var i = 0; i < faker.Random.Int(1, 2); i++)
                {
                    prov.AddOrUpdateCustomField(
                        key:   faker.Commerce.Department(),
                        value: faker.Commerce.ProductAdjective()
                    );
                }
            }

            await ctx.Providers.AddRangeAsync(providers);
            await ctx.SaveChangesAsync();
        }

        if (!await ctx.Users.AnyAsync())
        {
            var userFaker = new Faker<User>()
                .CustomInstantiator(f => User.Create(
                    username: f.Internet.UserName(),
                    plainPassword: "Pass@123",
                    email: f.Internet.Email(),
                    hasher: new Services.PasswordHasherService()
                ));

            var users = userFaker.Generate(10);
            await ctx.Users.AddRangeAsync(users);
            await ctx.SaveChangesAsync();
        }
    }
}