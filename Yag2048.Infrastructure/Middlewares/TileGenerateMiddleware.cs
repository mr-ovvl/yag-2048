using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

public class TileGenerateMiddleware : IGameStepMiddleware
{
    public Task Execute(IGameContext gameContext)
    {
        if (gameContext.GameStatus == GameStatus.Running)
        {
            if (gameContext.TileBoard.IsMoved)
                gameContext.TileBoard.AddRandomTile();
            gameContext.TileBoard.ResetFlags();
        }

        return Task.CompletedTask;
    }
}