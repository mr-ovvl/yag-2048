namespace Yag2048.Core.Board;

public interface ITileBoard
{
    int Width { get; }

    int Height { get; }

    int MaxTileValue { get; }

    bool IsMoved { get; }

    ITileBoard AddRandomTile();

    IEnumerable<ITile> GetTiles();

    ITile GetTile(Position position);

    ITile GetTile(int row, int col);

    bool CanMove();

    ITileBoard Move(MoveDirection direction);

    ITileBoard Reset();

    ITileBoard ResetFlags();
}