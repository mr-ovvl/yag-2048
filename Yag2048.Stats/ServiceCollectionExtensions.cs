using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Yag2048.Core.Stats;

namespace Yag2048.Stats;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStats(this IServiceCollection services)
    {
        services.AddSingleton<Func<int, long, TimeSpan, ITileStats>>(_ => TileStats.Create);
        services.AddSingleton<IStats, LocalStats>();
        services.AddSingleton<Stopwatch>();
        return services;
    }
}