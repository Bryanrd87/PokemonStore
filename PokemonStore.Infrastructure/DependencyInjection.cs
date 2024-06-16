using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using PokemonStore.Domain.Interfaces;
using PokemonStore.Infrastructure.Data;
using PokemonStore.Infrastructure.Repositories;

namespace PokemonStore.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
    this IServiceCollection services)
    {
        services.AddDbContext<PokemonDbContext>(options =>
        {
            options.UseInMemoryDatabase("PokemonStore")
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        });

        services.AddScoped<IPokemonRepository, PokemonRepository>();

        return services;
    }
}
