using Yag2048.Game.Board;

namespace Yag2048.Game.Rules2048.Game;

public class FinishRule2048 : FinishRule
{
    protected override int WinTileValue => 2048;
}