namespace TranscribeMe.ViewModel.Pages {

    [AddINotifyPropertyChangedInterface]
    public class ImagePageViewModel {

        public string? FilePath { get; set; }

        public string? DocPath { get; set; }

        public bool CanStartWorkButtonBePressed { get; set; }

        public Dictionary<string, string>? LanguagesDictionary { get; set; }

        public string? MicrosofWordtDocumentPath { get; set; }

        public Visibility ProcessMsgVisibility { get; set; }

        public Visibility MicrosofWordPathVisibility { get; set; }

        public Command PickFileCommad { get; set; }

        public Command OpenDocxCommnd { get; set; }

        public Command CopyDocumentPathCommand { get; set; }

        public AsyncCommand StartCommand { get; set; }

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

        public ImagePageViewModel() {

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
            System.Windows.Clipboard.SetText(MicrosofWordtDocumentPath);
        }

        private async Task StartAction() {
            IsBusy = true;
            ProcessMsgVisibility = Visibility.Visible;
            MicrosofWordPathVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = false;

            var textFromImage = await AzureOcrService.GiveTextAsync(FilePath!);

            DocPath = WordDocumentHelper.CreateWordDocument(textFromImage,
                Path.GetFileNameWithoutExtension(FilePath)!, "Ocr", false);

            IsBusy = false;
            ToastHelper.LaunchToastNotification(DocPath);
            ProcessMsgVisibility = Visibility.Hidden;
            CanStartWorkButtonBePressed = true;
            MicrosofWordPathVisibility = Visibility.Visible;
            MicrosofWordtDocumentPath = DocPath;
        }

        private bool CanStartAction(object arg) {
            return !string.IsNullOrEmpty(FilePath) &&
                   !IsBusy;
        }

        private void PickFileAction() {
            var FullPath = DialogHelper.GetFilePath(ConstantsHelpers.IMAGES);
            FilePath = FullPath;

            StartCommand?.RaiseCanExecuteChanged();
        }
    }
}
