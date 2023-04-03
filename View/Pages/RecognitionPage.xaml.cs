namespace TranscribeMe.View.Pages {
    /// <summary>
    /// Interaction logic for RecognitionPage.xaml
    /// </summary>
    public partial class RecognitionPage : Page {

        public static RecognitionViewModel Instance { get; } = new();
        public RecognitionPage() {
            InitializeComponent();
            DataContext = Instance;
        }
    }
}
