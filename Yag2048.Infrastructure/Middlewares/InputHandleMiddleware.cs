using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

public class InputHandleMiddleware : IGameStepMiddleware
{
    private readonly IInputHandler _inputHandler;

    public InputHandleMiddleware(IInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public Task Execute(IGameContext context)
    {
        context.GameAction = _inputHandler.GetAction();
        context.GameStatus = context.GameAction switch
        {
            GameAction.StartGame => GameStatus.Running,
            GameAction.ShowSettings => GameStatus.Settings,
            GameAction.ChangeMode => GameStatus.ModeChanging,
            _ => context.GameStatus
        };

        return Task.CompletedTask;
    }
}