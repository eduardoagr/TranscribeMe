
namespace TranscribeMe.ViewModel;

[AddINotifyPropertyChangedInterface]
public class MainWindowViewModel {
    public Command ExitCommand { get; set; }

    public Command AzureCommand { get; set; }

    public ObservableCollection<Tile>? Tiles { get; set; }

    public List<char> Words { get; set; }

    public MainWindowViewModel() {
        AzureCommand = new Command(AzureActionAsync);
        ExitCommand = new Command(ExiAction);
        InitCollection();
        Words = new List<char>();
    }

    private void InitCollection() {
        Tiles = new ObservableCollection<Tile>()
        {
            new Tile(){
                IsTileActive = true,
                TileTitle = Lang.AudioToText,
                TileIdentifier = (int)TilesIdentifiers.Audio,
                TileCommand = AzureCommand,
                TileIcon = IconFont.VolumeHigh
            },
             new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.Video,
                TileTitle = Lang.VideoToText,
                TileCommand = AzureCommand,
                TileIcon = IconFont.FileVideo
            },
              new Tile(){
                IsTileActive = true,
                TileTitle = Lang.ImageToText,
                TileIdentifier = (int)TilesIdentifiers.Ocr,
                TileCommand = AzureCommand,
                TileIcon = IconFont.EyeCircle
            },
                 new Tile(){
                IsTileActive = true,
                TileTitle = Lang.TranslateDocument,
                TileIdentifier = (int)TilesIdentifiers.document,
                TileCommand = AzureCommand,
                TileIcon = IconFont.FileDocument

            },
               new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.Account,
                TileTitle = Lang.Account,
                TileCommand = AzureCommand,
                TileIcon = IconFont.Account
            },
              new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.About,
                TileTitle = Lang.About,
                TileCommand = AzureCommand,
                TileIcon = IconFont.Help
            }
        };
    }

    private void ExiAction() {
        Application.Current.Shutdown();
    }

    private async void AzureActionAsync(object obj) {
        OpenFileDialog dlg;
        var AudioFolderPath = CreateFolder(ConstantsHelpers.AUDIO);
        const string ext = ".wav";

        switch (obj) {
            case (int)TilesIdentifiers.Audio:

                var AudioName = CreateDialog(out dlg, ConstantsHelpers.AUDIO);

                var Audiofilename = Path.Combine(AudioFolderPath, $"{AudioName}{ext}");

                Converter(dlg, Audiofilename, out _, out _);

                await ConvertToTextAsync(Audiofilename, (int)TilesIdentifiers.Audio);

                break;

            case (int)TilesIdentifiers.Video:

                var VideoName = CreateDialog(out dlg, ConstantsHelpers.VIDEO);

                var VideoFilename = Path.Combine(AudioFolderPath, $"{VideoName}{ext}");

                var inputFile = new MediaFile { Filename = dlg.FileName };
                var outputFile = new MediaFile { Filename = VideoFilename };
                var options = new ConversionOptions {
                    AudioSampleRate = AudioSampleRate.Hz22050
                };
                var engine = new Engine();

                if (!string.IsNullOrEmpty(inputFile.Filename)) {
                    engine.Convert(inputFile, outputFile, options);
                }

                break;

            case (int)TilesIdentifiers.Ocr:
                break;

            case (int)TilesIdentifiers.Account:
                Debug.WriteLine("Account", "Debug");
                break;

            case (int)TilesIdentifiers.document:

                var storageService = new AzureStorageService();

                CreateDialog(out dlg, ConstantsHelpers.DOCUMENTS);

                var path = CreateFolder(ConstantsHelpers.TRANSLATIONS);

                if (!string.IsNullOrEmpty(dlg.FileName)) {

                    var sourceUri = await storageService.UploadToAzureBlobStorage(Path.GetFullPath(dlg.FileName));

                    var targetUri = await storageService.SaveFromdAzureBlobStorage(Path.GetFullPath(dlg.FileName));

                    await AzureTranslationService.TranslatorAsync(sourceUri, targetUri);

                    var PathToSave = Path.Combine(path, dlg.FileName);

                    //using var client = new HttpClient();
                    //using var s = await client.GetStreamAsync(targetUri);
                    //using var fs = new FileStream(PathToSave, FileMode.OpenOrCreate);
                    //await s.CopyToAsync(fs);
                }

                break;

            case (int)TilesIdentifiers.About:
                Debug.WriteLine("about", "Debug");

                break;
        }
    }

    private void Converter(OpenFileDialog dlg, string filename, out Mp3FileReader? mp3, out WaveStream? ws) {
        if (!string.IsNullOrEmpty(dlg.FileName)) {
            mp3 = new Mp3FileReader(dlg.FileName);
            ws = WaveFormatConversionStream.CreatePcmStream(mp3);
            WaveFileWriter.CreateWaveFile(filename, ws);
        } else {
            mp3 = null;
            ws = null;
            return;
        }
    }

    private string? CreateDialog(out OpenFileDialog dlg, string type) {
        var filter = string.Empty;

        switch (type) {
            case ConstantsHelpers.AUDIO:
                filter = ConstantsHelpers.AUDIOFILES;
                break;
            case ConstantsHelpers.VIDEO:
                filter = ConstantsHelpers.VIDEOFILES;
                break;
            case ConstantsHelpers.DOCUMENTS:
                filter = ConstantsHelpers.DOCUMENTSFIILES;
                break;
            case ConstantsHelpers.IMAGES:
                filter = ConstantsHelpers.IMAGEFILES;
                break;
            default:
                break;
        }

        dlg = new OpenFileDialog {
            Filter = filter,
        };

        var res = dlg.ShowDialog();

        if (res == true) {
            return Path.GetFileNameWithoutExtension(dlg.FileName);
        }

        return null;
    }

    private static string CreateFolder(string FolderName = ConstantsHelpers.AUDIO) {
        var directoryPath = Directory.CreateDirectory(Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                                ConstantsHelpers.TRANSCRIBEME, FolderName));

        return directoryPath.FullName;
    }


    private async Task ConvertToTextAsync(string FilePath, int TileIndexWorking) {
        //Configure speech service

        var config = SpeechConfig.FromSubscription(ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

        config.EnableDictation();

        //Configure speech recognition

        var taskCompleteionSource = new TaskCompletionSource<int>();

        using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
        using var speechRecognizer = new SpeechRecognizer(config, audioConfig);

        speechRecognizer.Recognized += (sender, e) => {
            if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                foreach (var item in e.Result.Text) {
                    Words.Add(item);
                }
            }
        };

        speechRecognizer.SessionStarted += (sender, e) => {

            Tiles![TileIndexWorking].IsTileActive = false;

            Debug.WriteLine("Session started");
        };

        speechRecognizer.SessionStopped += (sender, e) => {

            Tiles![TileIndexWorking].IsTileActive = true;

            const string ext = ".docx";

            var pathToSave = CreateFolder(ConstantsHelpers.TRANSCRIPTIONS);

            var filename = Path.Combine(pathToSave, $"{Path.GetFileNameWithoutExtension(FilePath)}{ext}");

            var sb = new StringBuilder();

            foreach (var item in Words) {
                sb.Append(item);
            }

            using var document = new WordDocument();

            document.EnsureMinimal();

            document.LastParagraph.AppendText(sb.ToString());

            // Find all the text which start with capital letters next to period (.) in the Word document.

            //For example . Text or .Text

            TextSelection[] textSelections = document.FindAll(new Regex(@"[.]\s+[A-Z]|[.][A-Z]"));

            for (int i = 0; i < textSelections.Length; i++) {

                WTextRange textToFind = textSelections[i].GetAsOneRange();

                //Replace the period (.) with enter(\n).

                string replacementText = textToFind.Text.Replace(".", ".\n\n");

                textToFind.Text = replacementText;

            }

            document.Save(filename);

            CreateAndShowPrompt("Your document is ready");
        };

        await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

        Task.WaitAny(new[] { taskCompleteionSource.Task });

        await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    }

    private void CreateAndShowPrompt(string message) {
        var xml = $"<?xml version=\"1.0\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{message}</text></binding></visual></toast>";
        var toastXml = new XmlDocument();
        toastXml.LoadXml(xml);
        var toast = new ToastNotification(toastXml);

        ToastNotificationManager.CreateToastNotifier("Sample toast").Show(toast);
    }
}
