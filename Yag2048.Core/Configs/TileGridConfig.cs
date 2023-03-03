namespace Yag2048.Core.Configs;

public sealed class TileGridConfig
{
    public const int DefaultWidth = 4;
    public const int DefaultHeight = 4;

    public const int MinWidth = 2;
    public const int MinHeight = 2;

    public const int MaxWidth = 100;
    public const int MaxHeight = 100;

    public int Width { get; set; } = 4;

    public int Height { get; set; } = 4;
}