using Yag2048.Core.Game;

namespace Yag2048.Core.Infrastructure;

public interface IGameStepMiddleware
{
    Task Execute(IGameContext gameContext);
}