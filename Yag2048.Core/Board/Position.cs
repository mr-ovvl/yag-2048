namespace Yag2048.Core.Board;

public readonly struct Position
{
    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; init; }

    public int Col { get; init; }

    public Position GetNext(MoveDirection direction) =>
        direction switch
        {
            MoveDirection.Up => new Position(Row - 1, Col),
            MoveDirection.Down => new Position(Row + 1, Col),
            MoveDirection.Left => new Position(Row, Col - 1),
            MoveDirection.Right => new Position(Row, Col + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public override int GetHashCode()
    {
        unchecked
        {
            return (Col * 397) ^ Row;
        }
    }

    public override string ToString() => $"{{ {nameof(Row)}: {Row}, {nameof(Col)}: {Col} }}";
}