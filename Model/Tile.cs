
using System.Windows.Media;

using MahApps.Metro.IconPacks;

using MvvmHelpers.Commands;

namespace Model;

public class Tile
{
    public string? TileTitle
    {
        get; set;
    }

    public PackIconMaterialKind? TileIcon
    {
        get; set;
    }

    public Color? TileColor
    {
        get; set;
    }
    public Command? Command
    {
        get; set;
    }
    private bool _IsEnable;
    public bool IsEnable
    {
        get => _IsEnable;
        set
        {
            if (_IsEnable != value)
            {
                _IsEnable = value;
                SetColor();
            }
        }
    }

    private void SetColor()
    {
        if (IsEnable)
        {
            TileColor = TileColor;
        }
        else
        {
            TileColor = Color.FromRgb(192, 192, 192);
        }
    }
}
