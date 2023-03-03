namespace Yag2048.Core.Game;

public interface IFinishRule
{
    bool IsWin(IGameContext gameContext);

    bool IsLose(IGameContext gameContext);
}