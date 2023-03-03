using Yag2048.Core.Board;

namespace Yag2048.Game.Rules2048.Board;

public class TileSliceMover2048 : ITileSliceMover
{
    public bool Move(ITileSlice slice, ITileGrid grid, MoveDirection direction)
    {
        bool isMoved = false;
        foreach (var tile in slice.Tiles)
        {
            if (tile.CanMove(grid, direction))
                isMoved = true;
            while (tile.CanMove(grid, direction))
                tile.Move(grid, direction);

            var nextPosition = tile.Position.GetNext(direction);
            if (grid.IsWithin(nextPosition) && tile.CanCollide(grid[nextPosition]))
            {
                tile.Collide(grid, grid[nextPosition]);
                isMoved = true;
            }
        }

        return isMoved;
    }
}