
using System.Windows.Media;

using MvvmHelpers.Commands;

using PropertyChanged;

namespace Model;

[AddINotifyPropertyChangedInterface]
public class Tile
{
    public string TileTitle
    {
        get; set;
    }

    public string TileIcon
    {
        get; set;
    }
    public Brush TileColor
    {
        get; set;
    }
    public Command TileCommand
    {
        get; set;
    }
    private bool _IsTileActive;
    public bool IsTileActive
    {
        get => _IsTileActive;
        set
        {
            if (_IsTileActive != value)
            {
                _IsTileActive = value;
                SetColor();
            }
        }
    }

    private void SetColor()
    {
        if (IsTileActive)
        {
            TileColor = TileColor;
        }
        else
        {
            TileColor = new SolidColorBrush(Color.FromRgb(192, 192, 192));
        }
    }
}
