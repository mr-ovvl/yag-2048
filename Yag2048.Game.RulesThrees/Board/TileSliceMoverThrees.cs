using Yag2048.Core.Board;

namespace Yag2048.Game.RulesThrees.Board;

public class TileSliceMoverThrees : ITileSliceMover
{
    public bool Move(ITileSlice slice, ITileGrid grid, MoveDirection direction)
    {
        bool isMoved = false;
        foreach (var tile in slice.Tiles)
        {
            if (tile.CanMove(grid, direction))
            {
                isMoved = true;
                tile.Move(grid, direction);
            }
            else
            {
                var nextPosition = tile.Position.GetNext(direction);
                if (grid.IsWithin(nextPosition) && tile.CanCollide(grid[nextPosition]))
                {
                    tile.Collide(grid, grid[nextPosition]);
                    isMoved = true;
                }
            }
        }

        return isMoved;
    }
}