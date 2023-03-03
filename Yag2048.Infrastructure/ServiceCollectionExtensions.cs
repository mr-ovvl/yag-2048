using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;
using Yag2048.Infrastructure.Middlewares;

namespace Yag2048.Infrastructure;

public static class ServiceCollectionExtension
{
    private const string DefaultFileName = "appsettings.json";

    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddSingleton<IGameStepMiddleware, RenderMiddleware>();
        services.AddSingleton<IGameStepMiddleware, InputHandleMiddleware>();
        services.AddSingleton<IGameStepMiddleware, MoveBoardMiddleware>();
        services.AddSingleton<IGameStepMiddleware, StatsCalculationMiddleware>();
        services.AddSingleton<IGameStepMiddleware, TileGenerateMiddleware>();
        services.AddSingleton<IGameStepMiddleware, UpdateStatusMiddleware>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IRenderer, SimpleConsoleRenderer>();
        services.AddTransient<IInputHandler, InputHandler>();
        services.AddSingleton<IGameContext, GameContext>();
        services.AddSingleton<IGameStepPipeline, GameStepPipeline>();
        return services;
    }

    public static IServiceCollection ConfigureWritable<TOptions>(
        this IServiceCollection services,
        IConfigurationSection section,
        string file = DefaultFileName)
        where TOptions : class, new() =>
        services.Configure<TOptions>(section)
            .AddTransient<IWritableOptions<TOptions>>(provider =>
            {
                var environment = provider.GetService<IHostEnvironment>();
                var jsonFilePath = environment?.ContentRootFileProvider.GetFileInfo(file).PhysicalPath
                                    ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

                var configuration = provider.GetService<IConfiguration>();
                var options = provider.GetRequiredService<IOptionsMonitor<TOptions>>();
                return new WritableOptions<TOptions>(jsonFilePath, section.Key, options, configuration);
            });

    public static IServiceCollection ConfigureWritableWithExplicitPath<TOptions>(
        this IServiceCollection services,
        IConfigurationSection section,
        string directoryPath,
        string file = DefaultFileName)
        where TOptions : class, new() =>
        services.Configure<TOptions>(section)
            .AddTransient<IWritableOptions<TOptions>>(provider =>
            {
                var jsonFilePath = Path.Combine(directoryPath, file);
                var configuration = provider.GetService<IConfigurationRoot>();
                var options = provider.GetRequiredService<IOptionsMonitor<TOptions>>();
                return new WritableOptions<TOptions>(jsonFilePath, section.Key, options, configuration);
            });
}