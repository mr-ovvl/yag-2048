namespace Yag2048.Core.Board;

public interface ITileGenerator
{
    ITile Generate(ITileGrid grid);

    ITile GenerateEmpty(Position position);
}