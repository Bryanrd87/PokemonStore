using MediatR.NotificationPublishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PokemonStore.Application.Configurations;
using PokemonStore.Application.Pokemon.Commands.Add;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace PokemonStore.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();

            config.NotificationPublisher = new TaskWhenAllPublisher();
        });

        services.AddHttpClient();

        services.AddValidatorsFromAssemblyContaining<AddPokemonCommandValidator>();
        services.AddFluentValidationAutoValidation();

        services.Configure<PokemonApiSettings>(configuration.GetSection("PokemonApiSettings"));

        services.AddLogging();

        services.AddSingleton<ILoggerFactory>(serviceProvider =>
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code);

            var logger = loggerConfiguration.CreateLogger();

            return new SerilogLoggerFactory(logger, dispose: true);
        });

        return services;
    }
}