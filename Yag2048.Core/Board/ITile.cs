namespace Yag2048.Core.Board;

public interface ITile
{
    Position Position { get; }

    int Value { get; }

    bool IsMerged { get; set; }

    bool IsEmpty { get; }

    bool CanCollide(ITile destination);

    bool CanMove(ITileGrid grid, Position position);

    bool CanMove(ITileGrid grid, MoveDirection direction);

    ITile Collide(ITileGrid grid, ITile destination);

    ITile Move(ITileGrid grid, Position position);

    ITile Move(ITileGrid grid, MoveDirection direction);

    string ToString();
}