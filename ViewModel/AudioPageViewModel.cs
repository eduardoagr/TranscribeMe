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

        public WordDocumentHelper DocumentHelper { get; set; }

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
            LanguagesDictionary = LanguagesHelper.GetLanguages();
            DocumentHelper = new WordDocumentHelper();
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

            var AudioFileNamePath = Path.Combine(AudioFolderPath, $"{FileWithoutExtension}{ConstantsHelpers.WAV}");

            var ConvertedAudioPath = AudioHelper.Converter(FilePath!, AudioFileNamePath);

            // var str = await AzureTranscription.ConvertToTextAsync(ConvertedAudioPath,
            //FileWithoutExtension!, SelectedLanguage);

            var str = DocumentHelper.CreateWordDocument(" You understand?Yeah. I mean, of course you'd say that.Such a typical thing for a therapist to say.You know what? Let's just cut the crap, all right? I'm not stupid. I know that you could care less about me or my problems, and all you really care about is getting that money at the end of the session.So you know what? Let's make a deal. You know, I'm fine with just sitting here an hour a week on my phone. You can do whatever it is that you do. Then my mom will be happy because I'm here.And you'll be happy because he gets your money.Now do we have a deal?Or would you rather just keep playing this fake sympathetic therapist that pretends to care about her clients?And.And I'll just go along being the gullible patient who thinks that I finally.Have someone who wants to help me?Either way.Get your money.And I understand that's all you really want.End scene.", SelectedLanguage);
            IsBusy = false;
            ProcessMsgVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = true;
            MicrosofWordPathVisibility = Visibility.Visible;
            //MicrosofWordtDocumentPath = DocumentName;
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
    }
}
