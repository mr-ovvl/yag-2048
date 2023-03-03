namespace Yag2048.Core.Board;

public interface ITileSliceMover
{
    bool Move(ITileSlice slice, ITileGrid grid, MoveDirection direction);
}