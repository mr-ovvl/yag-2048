using Yag2048.Core;
using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

public class UpdateStatusMiddleware : IGameStepMiddleware
{
    private readonly Func<IFinishRule> _getFinishRule;

    public UpdateStatusMiddleware(Func<IFinishRule> getFinishRule)
    {
        _getFinishRule = getFinishRule;
    }

    public Task Execute(IGameContext gameContext)
    {
        if (gameContext.GameAction == GameAction.QuitGame)
        {
            gameContext.GameStatus = GameStatus.Quit;
            return Task.CompletedTask;
        }

        if (gameContext.GameStatus == GameStatus.ModeChanging)
        {
            gameContext.Mode = GetGameMode(gameContext.Mode);
            Utils.Initialize(gameContext);
        }

        if (gameContext.GameStatus == GameStatus.Win
            && gameContext.GameAction != GameAction.QuitGame)
        {
            if (gameContext.GameAction == GameAction.Confirm)
            {
                gameContext.GameStatus = GameStatus.Running;
                gameContext.IsEndless = true;
                gameContext.Stopwatch.Start();
            }

            if (gameContext.GameAction == GameAction.Cancel)
            {
                gameContext.Init();
                gameContext.GameStatus = GameStatus.Running;
            }

            return Task.CompletedTask;
        }

        if (gameContext.GameStatus == GameStatus.Lose
            && gameContext.GameAction != GameAction.QuitGame)
        {
            if (gameContext.GameAction == GameAction.Confirm)
            {
                gameContext.Init();
                gameContext.GameStatus = GameStatus.Running;
            }

            if (gameContext.GameAction == GameAction.Cancel)
            {
                gameContext.GameStatus = GameStatus.Quit;
            }

            return Task.CompletedTask;
        }

        var rule = _getFinishRule();

        if (rule.IsWin(gameContext))
        {
            gameContext.GameStatus = GameStatus.Win;
            gameContext.Stopwatch.Stop();
        }

        if (rule.IsLose(gameContext))
        {
            gameContext.GameStatus = GameStatus.Lose;
            gameContext.Stopwatch.Stop();
        }

        return Task.CompletedTask;
    }

    private GameMode GetGameMode(GameMode current) =>
        current switch
        {
            GameMode.Game2048 => GameMode.GameThrees,
            GameMode.GameThrees => GameMode.Game2048,
            _ => current
        };
}