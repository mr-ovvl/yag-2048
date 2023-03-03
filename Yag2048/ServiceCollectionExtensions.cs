using Microsoft.Extensions.DependencyInjection;
using Yag2048.Core.Board;
using Yag2048.Core.Game;
using Yag2048.Game.Board;
using Yag2048.Game.Rules2048.Board;
using Yag2048.Game.Rules2048.Game;
using Yag2048.Game.RulesThrees.Board;
using Yag2048.Game.RulesThrees.Game;

namespace Yag2048;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTileValueFactory(this IServiceCollection services)
    {
        services.AddSingleton<Random>();
        services.AddSingleton<Func<int>>(serviceProvider => () => serviceProvider.GetRequiredService<IGameContext>().Mode switch
        {
            GameMode.Game2048 => serviceProvider.GetRequiredService<Random>().Next(10) > 8 ? 4 : 2,
            GameMode.GameThrees => serviceProvider.GetRequiredService<Random>().Next(10) > 5 ? 1 : 2,
            _ => throw new ArgumentOutOfRangeException(nameof(GameMode))
        });
        return services;
    }

    public static IServiceCollection AddTileColliderProvider(this IServiceCollection services) =>
        services.AddSingleton<Func<ITileCollider>>(serviceProvider => () =>
            serviceProvider.GetRequiredService<IGameContext>().Mode switch
            {
                GameMode.Game2048 => serviceProvider.GetRequiredService<TileCollider2048>(),
                GameMode.GameThrees => serviceProvider.GetRequiredService<TileColliderThrees>(),
                _ => throw new ArgumentOutOfRangeException(nameof(GameMode))
            });

    public static IServiceCollection AddTileSliceMoverProvider(this IServiceCollection services) =>
        services.AddSingleton<Func<ITileSliceMover>>(serviceProvider => () =>
            serviceProvider.GetRequiredService<IGameContext>().Mode switch
            {
                GameMode.Game2048 => serviceProvider.GetRequiredService<TileSliceMover2048>() as ITileSliceMover,
                GameMode.GameThrees => serviceProvider.GetRequiredService<TileSliceMoverThrees>() as ITileSliceMover,
                _ => throw new ArgumentOutOfRangeException(nameof(GameMode))
            });

    public static IServiceCollection AddGameRulesProvider(this IServiceCollection services) =>
        services.AddSingleton<Func<IFinishRule>>(serviceProvider => () =>
        {
            var gameContext = serviceProvider.GetRequiredService<IGameContext>();
            return gameContext.IsEndless
                ? serviceProvider.GetRequiredService<FinishRule>()
                : gameContext.Mode switch
                {
                    GameMode.Game2048 => serviceProvider.GetRequiredService<FinishRule2048>(),
                    GameMode.GameThrees => serviceProvider.GetRequiredService<FinishRuleThrees>(),
                    _ => throw new ArgumentOutOfRangeException(nameof(GameMode))
                };
        });
}