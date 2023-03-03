using Microsoft.Extensions.DependencyInjection;
using Yag2048.Game.Rules2048.Board;
using Yag2048.Game.Rules2048.Game;

namespace Yag2048.Game.Rules2048;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRules2048(this IServiceCollection services)
    {
        services.AddSingleton<TileSliceMover2048>();
        services.AddSingleton<TileCollider2048>();
        services.AddSingleton<FinishRule2048>();
        return services;
    }
}