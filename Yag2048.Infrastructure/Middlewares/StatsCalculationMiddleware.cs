using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

internal class StatsCalculationMiddleware : IGameStepMiddleware
{
    public Task Execute(IGameContext gameContext)
    {
        if (gameContext.GameStatus == GameStatus.Running && gameContext.TileBoard.IsMoved)
        {
            var stepScore = gameContext.TileBoard.GetTiles()
                .Where(x => x.IsMerged)
                .Sum(x => x.Value);

            gameContext.Stats.Update(gameContext.Stats.Score + stepScore, gameContext.TileBoard.MaxTileValue, gameContext.Stopwatch.Elapsed);
        }

        return Task.CompletedTask;
    }
}
