using ModernWpf.Controls.Primitives;

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

        public List<string> Options { get; set; }

        public Command<string> GenerateCommand { get; set; }

        private bool _isRecognizing;

        public string? RecognizeText { get; set; }

        public string? SelectedLanguage { get; set; }


        public RecognitionViewModel() {

            Options = new List<string>() { "pdf", "docx" };

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

                var box = new TextBox {
                    Height = 30.0
                };
                ControlHelper.SetPlaceholderText(box, Lang.DocName);

                var dialog = new ContentDialog {
                    Content = box,
                    PrimaryButtonText = "OK",
                    SecondaryButtonText = "Cancel",
                    IsPrimaryButtonEnabled = !string.IsNullOrEmpty(box.Text)
                };

                box.TextChanged += (sender, args) => {
                    dialog.IsPrimaryButtonEnabled = !string.IsNullOrEmpty(box.Text);
                };

                dialog.PrimaryButtonClick += (sender, args) => {

                    WordDocumentHelper.CreateWordDocument(text, box.Text, "Documents",
                        false);
                };

                dialog.Content = box;

                await dialog.ShowAsync();

            } else {
                var dialog = new ContentDialog() {

                    Title = Lang.ErrorDialog,
                    Content = Lang.Document,
                    PrimaryButtonText = "OK"
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
