namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]

    public class TranslatePageViewModel {

        public string? FilePath { get; set; }

        public string? DocPath { get; set; }

        public bool CanStartWorkButtonBePressed { get; set; }

        public Dictionary<int, Languages>? LanguagesDictionary { get; set; }

        public string? DocumentPath { get; set; }

        public Visibility ProcessMsgVisibility { get; set; }

        public Visibility PathVisibility { get; set; }

        public Command PickFileCommad { get; set; }

        public Command OpenDocCommnd { get; set; }

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

        public TranslatePageViewModel() {

            LanguagesDictionary = LanguagesHelper.GetLanguages();
            ProcessMsgVisibility = Visibility.Hidden;
            PathVisibility = Visibility.Hidden;
            PickFileCommad = new Command(PickFileAction);
            StartCommand = new AsyncCommand(StartAction, CanStartAction);
            CopyDocumentPathCommand = new Command(CopyDocumentPathAction);
            OpenDocCommnd = new Command(OpenDocxAction);

            CanStartWorkButtonBePressed = true;
        }

        private void OpenDocxAction(object obj) {
            Process p = new();
            p.StartInfo.FileName = DocPath;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }


        private void CopyDocumentPathAction() {
            Clipboard.SetText(DocumentPath);
        }

        private async Task StartAction() {

            AzureStorageService azureStorageService = new();

            IsBusy = true;
            ProcessMsgVisibility = Visibility.Visible;
            PathVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = false;

            var inputDocument = Path.GetFullPath(FilePath!);

            var sourceUri = await azureStorageService.UploadToAzureBlobStorageWithssaToken
                (inputDocument!);

            var targetUri = await azureStorageService.GetTargetUri();

            var translated = await AzureTranslationService.TranslatorAsync(sourceUri, targetUri,
                SelectedLanguage);

            if (translated) {

                var translationsFolder = FolderHelper.CreateFolder(ConstantsHelpers.TRANSLATIONS);

                var PathToSave = Path.Combine(translationsFolder, Path.GetFileName(inputDocument));

                DocPath = await azureStorageService.DownloadFileFromBlobAsync(PathToSave, inputDocument);

                await azureStorageService.DeteleFromBlobAsync(inputDocument,
                    ConstantsHelpers.AZURE_CONTAINER_ORIGINAL_DOCUMENT);

                await azureStorageService.DeteleFromBlobAsync(inputDocument,
                    ConstantsHelpers.AZURE_CONTAINER_TRANSLATED_DOCUMENT);
            }

            IsBusy = false;
            ToastHelper.LaunchToastNotification(DocPath!);
            ProcessMsgVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = true;
            PathVisibility = Visibility.Visible;
            DocumentPath = DocPath;
        }

        private bool CanStartAction(object arg) {
            return !string.IsNullOrEmpty(SelectedLanguage) &&
                   !string.IsNullOrEmpty(FilePath) &&
                   !IsBusy;
        }

        private void PickFileAction() {
            var FullPath = DialogHelper.GetFilePath(ConstantsHelpers.DOCUMENTS);
            FilePath = FullPath;

            StartCommand?.RaiseCanExecuteChanged();
        }
    }
}
