using System.Windows.Media;

using MvvmHelpers.Commands;

namespace Model;

public class Tile
{
    public string? TileIdentifier
    {
        get; set;
    }
    public string? TileIcon
    {
        get; set;
    }
    public Brush? TileColor
    {
        get; set;
    }
    public Command? TileCommand
    {
        get; set;
    }
    public bool? IsTileActive
    {
        get; set;
    }
    public string? TileTitle
    {
        get; set;
    }
}
