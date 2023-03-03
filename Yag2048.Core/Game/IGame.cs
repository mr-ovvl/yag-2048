namespace Yag2048.Core.Game;

public interface IGame
{
    Task Run(CancellationToken cancellationToken = default);
}