

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class AudioPageViewModel {

        public string? FilePath { get; set; }

        public Dictionary<int, Languages>? LanguagesDictionary { get; set; }

        public string? DocumentPath { get; set; }

        public Visibility CanShow { get; set; } = Visibility.Collapsed;

        public DialogHelper DialogHelper { get; }

        public FolderHelper FolderHelper { get; }

        public AudioHelper AudioHelper { get; }

        public bool CanBePressed { get; set; }

        public AzureTranscriptionService AzureTranscription { get; }

        public Command PickFileCommad { get; set; }

        public Command CopyDocumentPathCommand { get; set; }

        public AsyncCommand StartCommand { get; set; }

        private string? _SelectedLanguage;

        public string SelectedLanguage {
            get => _SelectedLanguage!;
            set {
                if (_SelectedLanguage != value) {
                    _SelectedLanguage = value;
                    StartCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private bool _IsBusy;
        public bool IsBusy {
            get { return _IsBusy; }
            set {
                if (_IsBusy != value) {
                    _IsBusy = value;
                    StartCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public AudioPageViewModel() {
            InitListLanguages();
            CanBePressed = true;
            AzureTranscription = new AzureTranscriptionService();
            DialogHelper = new DialogHelper();
            FolderHelper = new FolderHelper();
            AudioHelper = new AudioHelper();
            CanShow = Visibility.Hidden;
            PickFileCommad = new Command(PickFileAction);
            StartCommand = new AsyncCommand(StartAction, CanStartAction);
            CopyDocumentPathCommand = new Command(CopyDocumentPathAction);
        }

        private void CopyDocumentPathAction() {
            throw new NotImplementedException();
        }

        private async Task StartAction() {
            IsBusy = true;
            CanShow = Visibility.Visible;
            CanBePressed = false;

            var FileWithoutExtension = Path.GetFileNameWithoutExtension
                (FilePath);

            var AudioPath = FolderHelper.CreateFolder(ConstantsHelpers.AUDIO);

            var DocumentPath = FolderHelper.CreateFolder();

            var AudioFileNamePath = Path.Combine(AudioPath, $"{FileWithoutExtension}{ConstantsHelpers.WAV}");

            var ConvertedAudioPath = AudioHelper.Converter(FilePath!, AudioFileNamePath);

            var DocumentName = Path.Combine(DocumentPath, $"{FileWithoutExtension}{ConstantsHelpers.DOCX}");

            //await AzureTranscription.ConvertToTextAsync(ConvertedAudioPath,
            //FileWithoutExtension!, SelectedItem);

            await Task.Delay(10000);

            IsBusy = false;
            CanShow = Visibility.Hidden;
            CanBePressed = true;
        }

        private bool CanStartAction(object arg) {
            return !string.IsNullOrEmpty(SelectedLanguage) &&
                   !string.IsNullOrEmpty(FilePath) &&
                   !IsBusy;
        }


        private void PickFileAction() {
            var FullPath = DialogHelper.GetFilePath(ConstantsHelpers.AUDIO);
            FilePath = FullPath;

            StartCommand?.RaiseCanExecuteChanged();
        }

        private void InitListLanguages() {

            LanguagesDictionary = new Dictionary<int, Languages>() {

                {1, new Languages { Name = "Spanish", Code = "es"} },
                {2, new Languages { Name = "English", Code = "en"} }
            };
        }
    }
}
