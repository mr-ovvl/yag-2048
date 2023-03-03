using Yag2048.Core.Game;
using Yag2048.Core.Infrastructure;

namespace Yag2048.Infrastructure.Middlewares;

public class RenderMiddleware : IGameStepMiddleware
{
    private readonly IRenderer _renderer;

    public RenderMiddleware(IRenderer renderer)
    {
        _renderer = renderer;
    }

    public Task Execute(IGameContext context)
    {
        _renderer.Render(context);
        return Task.CompletedTask;
    }
}