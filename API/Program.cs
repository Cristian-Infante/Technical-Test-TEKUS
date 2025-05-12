using System.Text.Json.Serialization;
using API.Extensions;
using Application.Commands.Handlers;
using Application.Interfaces;
using Application.Queries;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.External;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddHttpClient<ICountryRepository, WorldBankCountryService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddMediatR(typeof(GetCountriesQueryHandler));
builder.Services.AddMediatR(typeof(GetCountryByIsoQueryHandler));
builder.Services.AddMediatR(typeof(GetUsersQueryHandler));
builder.Services.AddMediatR(typeof(GetUserByIdQueryHandler));
builder.Services.AddMediatR(typeof(GetProvidersQueryHandler));
builder.Services.AddMediatR(typeof(GetServicesQueryHandler));
builder.Services.AddMediatR(typeof(GetServiceByIdQueryHandler));

builder.Services.AddMediatR(typeof(CreateUserCommandHandler));
builder.Services.AddMediatR(typeof(UpdateUserCommandHandler));
builder.Services.AddMediatR(typeof(DeleteUserCommandHandler));
builder.Services.AddMediatR(typeof(LoginCommandHandler));
builder.Services.AddMediatR(typeof(CreateProviderCommandHandler));
builder.Services.AddMediatR(typeof(DeleteProviderCommandHandler));
builder.Services.AddMediatR(typeof(CreateServiceCommandHandler));
builder.Services.AddMediatR(typeof(UpdateServiceCommandHandler));
builder.Services.AddMediatR(typeof(DeleteServiceCommandHandler));


builder.Services.AddJwtAuthentication(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("Infrastructure");
            sqlOptions.EnableRetryOnFailure();
        }
    ));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title            = "Gestión de servicios de proveedores",
        Version          = "v1",
        Description      = "API RESTful para administrar proveedores, servicios y métricas resumen.",
        Contact     = new OpenApiContact
        {
            Name       = "Cristian Infante",
            Email      = "",
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        In           = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description  = "Introduce tu token JWT:"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión de servicios de proveedores");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.Migrate();
    await DataSeeder.SeedAsync(ctx);
}

app.Run();
