using Yag2048.Core.Board;

namespace Yag2048.Game.Board;

public class Tile : ITile
{
    private readonly ITileCollider _tileCollider;

    public Tile(ITileCollider tileCollider, Position position, int value)
    {
        _tileCollider = tileCollider;
        Position = position;
        Value = value;
    }

    public Position Position { get; private set; }

    public int Value { get; set; }

    public bool IsMerged { get; set; }

    public bool IsEmpty => Value == default;

    public bool CanMove(ITileGrid grid, Position position) => grid.IsWithin(position) && grid.IsEmpty(position);

    public bool CanMove(ITileGrid grid, MoveDirection direction) => CanMove(grid, Position.GetNext(direction));

    public ITile Move(ITileGrid grid, Position position)
    {
        var oldPosition = Position;
        Position = position;
        grid.UpdatePosition(oldPosition, Position);
        return this;
    }

    public ITile Move(ITileGrid grid, MoveDirection direction) => Move(grid, Position.GetNext(direction));

    public bool CanCollide(ITile destination) => _tileCollider.CanCollide(this, destination);

    public ITile Collide(ITileGrid grid, ITile destination)
    {
        Value = _tileCollider.GetCollidedValue(this, destination);
        IsMerged = true;
        return Move(grid, destination.Position);
    }

    public override string ToString() =>
        $"{{ {nameof(Value)}: {Value}, {nameof(Position)}: {Position}, {nameof(IsMerged)}: {IsMerged} }}";
}