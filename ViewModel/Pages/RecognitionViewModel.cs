using TranscribeMe.View.Dialogs;

using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace TranscribeMe.ViewModel.Pages {

    [AddINotifyPropertyChangedInterface]
    public class RecognitionViewModel {

        private readonly SpeechToTextService _speechService;

        public string? BtonText { get; set; }

        public Brush BtonBackGround { get; set; }

        public AsyncCommand SpeechCommand { get; set; }

        public Dictionary<string, string>? LanguagesDictionary { get; set; }


        public Command<string> GenerateCommand { get; set; }

        public bool IsButtonEnabled { get; set; } = false;


        private bool _isRecognizing;

        public string? RecognizeText { get; set; }

        public string? SelectedLanguage { get; set; }


        public RecognitionViewModel() {


            LanguagesDictionary = LanguagesHelper.GetLanguages();

            SelectedLanguage = LanguagesDictionary.ElementAt(1).Value;

            _speechService = new SpeechToTextService();

            BtonText = Lang.Recognize;

            BtonBackGround = Brushes.Green;

            SpeechCommand = new AsyncCommand(SpeechCommandActon);

            GenerateCommand = new Command<string>(GenerateAtion);

        }


        private async void GenerateAtion(object obj) {

            var text = obj as string;

            if (!string.IsNullOrEmpty(text)) {

                var dlg = new GenerateFileDialog();

                dlg.DocNameTextBox.TextChanged += (sender, args) => {
                    dlg.PrimaryBton.IsEnabled = !string.IsNullOrEmpty(dlg.DocNameTextBox.Text);
                };

                dlg.PrimaryBton.Click += (sender, args) => {
                    string str = dlg.DocNameTextBox.Text;
                    WordDocumentHelper.CreateWordDocument(str, text, "Documents",
                        false);

                    dlg.Hide();
                };

                dlg.SecundaryBton.Click += (s, a) => {
                    dlg.Hide();
                };



            } else {
                var dialog = new ContentDialog() {

                    Title = Lang.ErrorDialog,
                    Content = Lang.Document,
                    PrimaryButtonText = "OK",
                };

                await dialog.ShowAsync();
            }

        }

        private async Task SpeechCommandActon() {

            if (!_isRecognizing) {

                _isRecognizing = true;
                BtonText = Lang.Stop;
                BtonBackGround = Brushes.Red;
                await _speechService.StartRecognitionAsync(SelectedLanguage!);
                _speechService!.Recognizer!.Recognized += (sender, e) => {
                    RecognizeText += e.Result.Text;
                };

            } else {
                _isRecognizing = false;
                BtonText = Lang.Recognize;
                BtonBackGround = Brushes.Green;
                _speechService.StopRecognition();
            }
        }
    }
}
