using System.Windows.Controls;

using TranscribeMe.CustomControls;
using TranscribeMe.ViewModel;

namespace TranscribeMe;

public partial class MainWindow : Window {

    public static MainWindowViewModel Instance { get; } = new();
    public MainWindow() {
        InitializeComponent();
        DataContext = Instance;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
        MouseDown += delegate {
            DragMove();
        };
    }

    private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        var SelectedItem = NavList.SelectedItem as NavButton;

        if (SelectedItem != null) {
            NavFrame.Navigate(SelectedItem.NavLink);
        }
    }
}
