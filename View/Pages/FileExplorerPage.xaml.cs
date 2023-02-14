namespace TranscribeMe.Pages {

    public partial class FileExplorerPage : Page {

        public FileExplorerPageViewModel Instance { get; set; }
        public FileExplorerPage() {
            InitializeComponent();
            Instance = new FileExplorerPageViewModel();
            DataContext = Instance;
        }
    }
}

