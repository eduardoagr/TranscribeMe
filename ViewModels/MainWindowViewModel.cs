using CommunityToolkit.Mvvm.ComponentModel;

namespace TranscribeMe.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{

    [ObservableProperty]
    public bool _IsVisible;
}
