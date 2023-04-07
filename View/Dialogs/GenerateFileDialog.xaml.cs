namespace TranscribeMe.View.Dialogs {

    public partial class GenerateFileDialog : ContentDialog {

        static private RecognitionViewModel? _recognitionViewModel;

        public GenerateFileDialog() {
            InitializeComponent();
            _recognitionViewModel = new RecognitionViewModel();
            DataContext = _recognitionViewModel;
        }
    }
}
