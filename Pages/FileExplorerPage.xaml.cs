namespace TranscribeMe.Pages {

    public partial class FileExplorerPage : Page {

        public FileExplorerViewModel Instance { get; set; }
        public FileExplorerPage() {
            InitializeComponent();
            Instance = new FileExplorerViewModel();
            DataContext = Instance;
        }
    }
}

