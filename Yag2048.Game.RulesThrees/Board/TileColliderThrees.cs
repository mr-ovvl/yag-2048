using Yag2048.Core.Board;

namespace Yag2048.Game.RulesThrees.Board;

public class TileColliderThrees : ITileCollider
{
    public bool CanCollide(ITile source, ITile destination) =>
        source.Value + destination.Value == 3
        || (source.Value >= 3
            && destination.Value >= 3
            && source.Value == destination.Value);
}