namespace TranscribeMe.ViewModel {
    [AddINotifyPropertyChangedInterface]
    public class AudioPageViewModel {
        public string? FilePath { get; set; }

        public Dictionary<int, Languages>? LanguagesDictionary { get; set; }

        public string? MicrosofWordtDocumentPath { get; set; }

        public Visibility ProcessMsgVisibility { get; set; }

        public Visibility MicrosofWordPathVisibility { get; set; }

        public DialogHelper DialogHelper { get; }

        public FolderHelper FolderHelper { get; }

        public AudioHelper AudioHelper { get; }

        public bool CanStartWorkButtonBePressed { get; set; }

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
            CanStartWorkButtonBePressed = true;
            AzureTranscription = new AzureTranscriptionService();
            DialogHelper = new DialogHelper();
            FolderHelper = new FolderHelper();
            AudioHelper = new AudioHelper();
            ProcessMsgVisibility = Visibility.Hidden;
            MicrosofWordPathVisibility = Visibility.Hidden;
            PickFileCommad = new Command(PickFileAction);
            StartCommand = new AsyncCommand(StartAction, CanStartAction);
            CopyDocumentPathCommand = new Command(CopyDocumentPathAction);
        }

        private void CopyDocumentPathAction() {
            Clipboard.SetText(MicrosofWordtDocumentPath);
        }

        private async Task StartAction() {
            IsBusy = true;
            ProcessMsgVisibility = Visibility.Visible;
            MicrosofWordPathVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = false;

            var FileWithoutExtension = Path.GetFileNameWithoutExtension
                (FilePath);

            var AudioFolderPath = FolderHelper.CreateFolder(ConstantsHelpers.AUDIO);

            var DocumentPath = FolderHelper.CreateFolder();

            var AudioFileNamePath = Path.Combine(AudioFolderPath, $"{FileWithoutExtension}{ConstantsHelpers.WAV}");

            var ConvertedAudioPath = AudioHelper.Converter(FilePath!, AudioFileNamePath);

            var DocumentName = Path.Combine(DocumentPath, $"{FileWithoutExtension}{ConstantsHelpers.DOCX}");

            var str = await AzureTranscription.ConvertToTextAsync(ConvertedAudioPath,
            FileWithoutExtension!, SelectedLanguage);

            IsBusy = false;
            ProcessMsgVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = true;
            MicrosofWordPathVisibility = Visibility.Visible;
            MicrosofWordtDocumentPath = DocumentName;
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
