using Yag2048.Core.Game;

namespace Yag2048.Core.Infrastructure;

public interface IGameStepPipeline
{
    IGameStepPipeline Add(Func<PipelineDelegate<IGameContext>, PipelineDelegate<IGameContext>> middleware);

    IGameStepPipeline Add(Func<IGameContext, Func<Task>, Task> middleware);

    IGameStepPipeline Add(IGameStepMiddleware middleware);

    Task Run(IGameContext gameContext);
}