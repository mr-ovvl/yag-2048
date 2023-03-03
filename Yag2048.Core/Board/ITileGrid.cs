namespace Yag2048.Core.Board;

public interface ITileGrid
{
    int Width { get; }

    int Height { get; }

    int TilesCount { get; }

    int MaxTileValue { get; }

    ITile this[Position position] { get; }

    ITileGrid Add(ITile tile);

    ITileGrid Clear();

    IEnumerable<ITile> GetTiles();

    IEnumerable<ITile> GetCol(int col);

    IEnumerable<ITile> GetRow(int row);

    ITileSlice GetSlice(TileSliceType type, int index);

    bool IsFull();

    bool IsEmpty();

    bool IsEmpty(Position position);

    bool IsEmpty(int row, int col);

    bool IsWithin(Position position);

    ITileGrid Remove(Position position);

    void ResetIsMergedFlag();

    ITileGrid UpdatePosition(Position oldPosition, Position newPosition);

    IEnumerable<Position> GetFreePositions();
}