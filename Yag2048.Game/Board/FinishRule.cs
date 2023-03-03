using Yag2048.Core.Game;

namespace Yag2048.Game.Board;

public class FinishRule : IFinishRule
{
    protected virtual int WinTileValue => int.MaxValue;

    public bool IsWin(IGameContext gameContext) => gameContext.TileBoard.MaxTileValue == WinTileValue;

    public bool IsLose(IGameContext gameContext) => !gameContext.TileBoard.CanMove();
}