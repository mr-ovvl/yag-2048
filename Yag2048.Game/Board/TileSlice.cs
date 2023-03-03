using Yag2048.Core.Board;

namespace Yag2048.Game.Board;

public class TileSlice : ITileSlice
{
    private readonly ITileSliceMover _sliceMover;

    public TileSlice(ITileSliceMover sliceMover, TileSliceType type, IEnumerable<ITile> tiles)
    {
        _sliceMover = sliceMover;
        Type = type;
        Tiles = tiles;
    }

    public TileSliceType Type { get; init; }

    public IEnumerable<ITile> Tiles { get; init; }

    public bool Move(ITileGrid grid, MoveDirection direction) => _sliceMover.Move(this, grid, direction);
}