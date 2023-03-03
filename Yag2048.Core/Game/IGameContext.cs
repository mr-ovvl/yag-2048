using System.Diagnostics;
using Yag2048.Core.Board;
using Yag2048.Core.Stats;

namespace Yag2048.Core.Game;

public interface IGameContext
{
    GameMode Mode { get; set; }

    bool IsEndless { get; set; }

    GameAction GameAction { get; set; }

    GameStatus GameStatus { get; set; }

    ITileBoard TileBoard { get; }

    Stopwatch Stopwatch { get; }

    IStats Stats { get; }

    bool IsRunning { get; }

    public void Init();
}