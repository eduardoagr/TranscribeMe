namespace TranscribeMe.Pages {
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : Page {

        public static VideoPageViewModel Instance { get; } = new();

        public VideoPage() {
            InitializeComponent();
            DataContext = Instance;
        }
    }
}
