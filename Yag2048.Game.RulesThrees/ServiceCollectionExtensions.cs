using Microsoft.Extensions.DependencyInjection;
using Yag2048.Game.RulesThrees.Board;
using Yag2048.Game.RulesThrees.Game;

namespace Yag2048.Game.RulesThrees;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameRulesThrees(this IServiceCollection services)
    {
        services.AddSingleton<TileSliceMoverThrees>();
        services.AddSingleton<TileColliderThrees>();
        services.AddSingleton<FinishRuleThrees>();
        return services;
    }
}