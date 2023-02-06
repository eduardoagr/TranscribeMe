namespace TranscribeMe;

public partial class MainWindow : Window {

    public static MainPageViewModel? ViewModel { get; set; }
    public MainWindow() {
        InitializeComponent();
        ViewModel = new MainPageViewModel();
        DataContext = ViewModel;

    }
}
