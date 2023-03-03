using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Yag2048.Core;
using Yag2048.Core.Board;
using Yag2048.Core.Game;
using Yag2048.Core.Stats;

namespace Yag2048.Infrastructure;

public class GameContext : IGameContext
{
    private readonly ILogger<GameContext> _logger;

    public GameContext(ILogger<GameContext> logger, Stopwatch stopwatch, ITileBoard tileBoard, IStats stats)
    {
        _logger = logger;
        Stopwatch = stopwatch;
        TileBoard = tileBoard;
        Stats = stats;
    }

    public GameMode Mode { get; set; }

    public bool IsEndless { get; set; }

    public GameAction GameAction { get; set; }

    public GameStatus GameStatus { get; set; }

    public ITileBoard TileBoard { get; }

    public Stopwatch Stopwatch { get; }

    public bool IsRunning => GameStatus != GameStatus.Quit;

    public IStats Stats { get; }

    public void Init()
    {
        _logger.LogDebug(Utils.StartMessage0, nameof(GameContext), nameof(Init));

        TileBoard.Reset();
        Stopwatch.Reset();
        GameAction = GameAction.Unknown;
        GameStatus = GameStatus.Initialized;
        Stopwatch.Start();

        _logger.LogDebug(Utils.FinishMessage0, nameof(GameContext), nameof(Init));
    }
}