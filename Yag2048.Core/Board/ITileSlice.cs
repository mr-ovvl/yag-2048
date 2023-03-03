namespace Yag2048.Core.Board;

public interface ITileSlice
{
    TileSliceType Type { get; init; }

    IEnumerable<ITile> Tiles { get; init; }

    bool Move(ITileGrid grid, MoveDirection direction);
}