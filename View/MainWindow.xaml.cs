namespace TranscribeMe;

public partial class MainWindow : Window {

    public static MainPageViewModel ViewModel { get; } = new MainPageViewModel();
    public MainWindow() {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
        MouseDown += delegate {
            DragMove();
        };
    }
}
