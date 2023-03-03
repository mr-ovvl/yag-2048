using Microsoft.Extensions.Logging;
using Yag2048.Core;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Game;

public class Game : IGame
{
    private readonly IGameContext _gameContext;
    private readonly IGameStepPipeline _gameStepPipeline;
    private readonly ILogger<Game> _logger;
    private readonly IEnumerable<IGameStepMiddleware> _middlewares;

    public Game(
        ILogger<Game> logger,
        IGameContext gameContext,
        IGameStepPipeline gameStepPipeline,
        IEnumerable<IGameStepMiddleware> middlewares
    )
    {
        _logger = logger;
        _gameContext = gameContext;
        _gameStepPipeline = gameStepPipeline;
        _middlewares = middlewares;
    }

    public async Task Run(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(Game), nameof(Run));

        ConfigurePipeline(_gameStepPipeline);
        Utils.Initialize(_gameContext);

        while (!cancellationToken.IsCancellationRequested && _gameContext.IsRunning)
        {
            await _gameStepPipeline.Run(_gameContext);
        }

        _logger.LogDebug(Utils.FinishMessage0, nameof(Game), nameof(Run));
    }

    private void ConfigurePipeline(IGameStepPipeline pipeline)
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(Game), nameof(ConfigurePipeline));

        foreach (var middleware in _middlewares)
            pipeline.Add(middleware);

        _logger.LogDebug(Utils.FinishMessage0, nameof(Game), nameof(ConfigurePipeline));
    }
}