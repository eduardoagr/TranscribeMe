namespace TranscribeMe.View.Dialogs {
    /// <summary>
    /// Interaction logic for GenerateFileDialog.xaml
    /// </summary>
    public partial class GenerateFileDialog : ContentDialog {

        private RecognitionViewModel _recognitionViewModel;
        public GenerateFileDialog() {
            InitializeComponent();
           _recognitionViewModel = new RecognitionViewModel();
            DataContext = _recognitionViewModel;
        }
    }
}
