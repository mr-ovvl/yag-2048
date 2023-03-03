using Microsoft.Extensions.DependencyInjection;
using Yag2048.Core.Board;
using Yag2048.Core.Game;
using Yag2048.Game.Board;

namespace Yag2048.Game;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGame(this IServiceCollection services)
    {
        services.AddSingleton<FinishRule>();
        services.AddSingleton<Func<TileSliceType, IEnumerable<ITile>, ITileSlice>>(serviceProvider =>
            (type, tiles) => new TileSlice(serviceProvider.GetRequiredService<Func<ITileSliceMover>>()(), type, tiles));
        services.AddSingleton<Func<ITileCollider, Position, int, ITile>>(_ =>
            (tileCollider, position, value) => new Tile(tileCollider, position, value));
        services.AddSingleton<ITileGenerator, TileGenerator>();
        services.AddSingleton<ITileGrid, TileGrid>();
        services.AddSingleton<ITileGridMover, TileGridMover>();
        services.AddSingleton<ITileBoard, TileBoard>();
        services.AddSingleton<IGame, Game>();
        return services;
    }
}