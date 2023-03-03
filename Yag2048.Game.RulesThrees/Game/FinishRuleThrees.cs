using Yag2048.Game.Board;

namespace Yag2048.Game.RulesThrees.Game;

public class FinishRuleThrees : FinishRule
{
    protected override int WinTileValue => 12288;
}