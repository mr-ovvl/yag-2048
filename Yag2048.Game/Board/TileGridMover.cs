using Yag2048.Core;
using Yag2048.Core.Board;

namespace Yag2048.Game.Board;

public class TileGridMover : ITileGridMover
{
    private readonly Func<ITileCollider> _getTileCollider;

    public TileGridMover(Func<ITileCollider> getTileCollider)
    {
        _getTileCollider = getTileCollider;
    }

    public bool CanMove(ITileGrid grid) =>
        !grid.IsFull()
        || CanMoveAnySlice(grid, TileSliceType.Horizontal)
        || CanMoveAnySlice(grid, TileSliceType.Vertical);

    public bool Move(ITileGrid grid, MoveDirection direction)
    {
        var isGridMoved = false;
        var sliceType = Utils.GetSliceType(direction);
        var startSliceIndex = Utils.GetStartSliceIndex(grid, direction);
        var endSliceIndex = Utils.GetEndSliceIndex(grid, direction);
        var currentSliceIndex = startSliceIndex;

        while (startSliceIndex <= endSliceIndex
                   ? currentSliceIndex <= endSliceIndex
                   : currentSliceIndex >= endSliceIndex)
        {
            var slice = grid.GetSlice(sliceType, currentSliceIndex);
            var isSliceMoved = slice.Move(grid, direction);
            currentSliceIndex += startSliceIndex <= endSliceIndex ? 1 : -1;

            if (!isGridMoved && isSliceMoved)
                isGridMoved = true;
        }

        return isGridMoved;
    }

    private bool CanMoveAnySlice(ITileGrid grid, TileSliceType sliceType)
    {
        for (var i = 0; i < grid.Width; i++)
        {
            var slice = grid.GetSlice(sliceType, i);
            if (CanMoveInSingleSlice(slice))
                return true;
        }

        return false;
    }

    private bool CanMoveInSingleSlice(ITileSlice slice)
    {
        var collider = _getTileCollider();
        var prev = slice.Tiles.First();
        foreach (var current in slice.Tiles.Skip(1))
        {
            if (collider.CanCollide(prev, current))
                return true;
            prev = current;
        }

        return false;
    }
}