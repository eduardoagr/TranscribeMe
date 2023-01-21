namespace TranscribeMe.Pages {

    public partial class FilesCollectionPage : Page {

        public FilesCollectionViewModel Intance { get; set; } = new FilesCollectionViewModel();

        public FilesCollectionPage() {
            InitializeComponent();
            DataContext = Intance;
        }
    }
}
