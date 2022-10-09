using TranscribeMe.ViewModel;

namespace TranscribeMe;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
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

    private void MenuList_SelectionChanged(object sender, EventArgs e) {

    }
}
