using Yag2048.Core.Game;

namespace Yag2048.Core.Infrastructure;

public interface IInputHandler
{
    GameAction GetAction();
}