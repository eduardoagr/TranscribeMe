namespace Model;

public class Tile
{
    public int? TileIdentifier
    {
        get; set;
    }
    public string? TileIcon
    {
        get; set;
    }
    public Brush? TileColor { get; set; } = new SolidColorBrush(Color.FromRgb(1, 135, 134));
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
