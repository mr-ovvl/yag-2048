using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure;

public class GameStepPipeline : IGameStepPipeline
{
    private readonly IList<Func<PipelineDelegate<IGameContext>, PipelineDelegate<IGameContext>>> _components =
        new List<Func<PipelineDelegate<IGameContext>, PipelineDelegate<IGameContext>>>();

    public IGameStepPipeline Add(Func<PipelineDelegate<IGameContext>, PipelineDelegate<IGameContext>> middleware)
    {
        _components.Add(middleware);
        return this;
    }

    public IGameStepPipeline Add(Func<IGameContext, Func<Task>, Task> middleware) => Add(next => context => middleware(context, () => next(context)));

    public IGameStepPipeline Add(IGameStepMiddleware command) =>
        Add(async (context, next) =>
        {
            await command.Execute(context);
            await next();
        });

    public Task Run(IGameContext context)
    {
        static Task RunPipeline(IGameContext ctx) => Task.CompletedTask;

        var pipeline = _components.Reverse()
            .Aggregate((PipelineDelegate<IGameContext>)RunPipeline, (current, item) => item(current));

        return pipeline(context);
    }
}