namespace TranscribeMe.Pages {

    public partial class FilesCollectionPage : Page {

        public FilesCollectionViewModel Instance { get; set; }
        public FilesCollectionPage() {
            InitializeComponent();
            Instance = new FilesCollectionViewModel();
            DataContext = Instance;
        }
    }
}

