namespace TranscribeMe.ViewModel {
    [AddINotifyPropertyChangedInterface]
    public class VideoPageViewModel {
        public string? FilePath { get; set; }

        public string? DocPath { get; set; }

        public bool CanStartWorkButtonBePressed { get; set; }

        public Dictionary<int, Languages>? LanguagesDictionary { get; set; }

        public string? MicrosofWordtDocumentPath { get; set; }

        public Visibility ProcessMsgVisibility { get; set; }

        public Visibility MicrosofWordPathVisibility { get; set; }

        public Command PickFileCommad { get; set; }

        public Command OpenDocxCommnd { get; set; }

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

        public VideoPageViewModel() {

            LanguagesDictionary = LanguagesHelper.GetLanguages();
            ProcessMsgVisibility = Visibility.Hidden;
            MicrosofWordPathVisibility = Visibility.Hidden;
            PickFileCommad = new Command(PickFileAction);
            StartCommand = new AsyncCommand(StartAction, CanStartAction);
            CopyDocumentPathCommand = new Command(CopyDocumentPathAction);
            OpenDocxCommnd = new Command(OpenDocxAction);

            CanStartWorkButtonBePressed = true;
        }

        private void OpenDocxAction(object obj) {
            Process p = new();
            p.StartInfo.FileName = DocPath;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }


        private void CopyDocumentPathAction() {
            Clipboard.SetText(MicrosofWordtDocumentPath);
        }

        private async Task StartAction() {
            IsBusy = true;
            ProcessMsgVisibility = Visibility.Visible;
            MicrosofWordPathVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = false;

            var file = Path.GetFullPath
                (FilePath!);

            var audioFolder = FolderHelper.CreateFolder(ConstantsHelpers.AUDIOS);

            var audioFile = AudioHelper.mp4ToWav(file, file);

            var AudioFileNamePath = Path.Combine(audioFolder, $"{Path.GetFileName(audioFile)}");

            if (!File.Exists(AudioFileNamePath)) {

                File.Move(audioFile, AudioFileNamePath);
            }

            var str = await AzureTranscriptionService.ConvertToTextAsync(AudioFileNamePath,
           Path.GetFileNameWithoutExtension(file)!, SelectedLanguage);

            var strCorrectd = await BingSpellCheckService.SpellingCorrector(str!, SelectedLanguage);

            DocPath = WordDocumentHelper.CreateWordDocument(strCorrectd,
                Path.GetFileNameWithoutExtension(file)!, true);

            IsBusy = false;
            ToastHelper.LaunchToastNotification(DocPath);
            ProcessMsgVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = true;
            MicrosofWordPathVisibility = Visibility.Visible;
            MicrosofWordtDocumentPath = DocPath;

        }

        private bool CanStartAction(object arg) {
            return !string.IsNullOrEmpty(SelectedLanguage) &&
                   !string.IsNullOrEmpty(FilePath) &&
                   !IsBusy;
        }

        private void PickFileAction() {
            var FullPath = DialogHelper.GetFilePath(ConstantsHelpers.VIDEO);
            FilePath = FullPath;

            StartCommand?.RaiseCanExecuteChanged();
        }
    }
}
