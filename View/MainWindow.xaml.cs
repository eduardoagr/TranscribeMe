using System.Windows.Controls;
using System.Windows.Navigation;

using TranscribeMe.CustomControls;
using TranscribeMe.ViewModel;

namespace TranscribeMe;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        DataContext = new MainWindowViewModel();
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

    private void NavFrame_ContentRendered(object sender, EventArgs e) {

        NavFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
    }
}
