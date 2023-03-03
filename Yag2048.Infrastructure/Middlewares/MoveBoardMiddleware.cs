using Yag2048.Core.Board;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

public class MoveBoardMiddleware : IGameStepMiddleware
{
    public Task Execute(IGameContext gameContext)
    {
        if (gameContext.GameStatus == GameStatus.Running && IsMove(gameContext.GameAction))
        {
            gameContext.TileBoard.Move(GetMoveDirection(gameContext.GameAction));
            if (!gameContext.TileBoard.CanMove())
                gameContext.GameStatus = GameStatus.Lose;
        }

        return Task.CompletedTask;
    }

    private static bool IsMove(GameAction action) =>
        action switch
        {
            GameAction.MoveUp => true,
            GameAction.MoveDown => true,
            GameAction.MoveLeft => true,
            GameAction.MoveRight => true,
            _ => false
        };

    private static MoveDirection GetMoveDirection(GameAction action) =>
        action switch
        {
            GameAction.MoveUp => MoveDirection.Up,
            GameAction.MoveDown => MoveDirection.Down,
            GameAction.MoveLeft => MoveDirection.Left,
            GameAction.MoveRight => MoveDirection.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
}