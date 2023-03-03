namespace Yag2048.Core.Board;

public interface ITileGridMover
{
    bool CanMove(ITileGrid grid);

    bool Move(ITileGrid grid, MoveDirection direction);
}