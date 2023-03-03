namespace Yag2048.Core.Board;

public interface ITileCollider
{
    bool CanCollide(ITile source, ITile destination) => source.Value == destination.Value;

    int GetCollidedValue(ITile source, ITile destination) => source.Value + destination.Value;
}