using Yag2048.Core.Game;

namespace Yag2048.Core.Infrastructure;

public interface IRenderer
{
    public void Render(IGameContext context);
}