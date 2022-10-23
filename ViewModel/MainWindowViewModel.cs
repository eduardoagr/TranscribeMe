namespace TranscribeMe.ViewModel;

public class MainWindowViewModel {


    public List<char> Words { get; set; }

    public AzureStorageService StorageService { get; set; }

    public AzureTranscriptionService AzureTranscription { get; set; }

    public MainWindowViewModel() {
        StorageService = new AzureStorageService();
        AzureTranscription = new AzureTranscriptionService();
        Words = new List<char>();
    }



    private void ExiAction() {
        Application.Current.Shutdown();
    }

    private async void AzureActionAsync(object obj) {
        OpenFileDialog dlg;
        //var AudioFolderPath = FolderHelper.CreateFolder(ConstantsHelpers.AUDIO);
        const string ext = ".wav";

        switch (obj) {
            case (int)TilesIdentifiers.Audio:

                var AudioName = CreateDialog(out dlg, ConstantsHelpers.AUDIO);

                //var Audiofilename = Path.Combine(AudioFolderPath, $"{AudioName}{ext}");

                //Converter(dlg, Audiofilename, out _, out _);



                break;

            case (int)TilesIdentifiers.Video:

                var VideoName = CreateDialog(out dlg, ConstantsHelpers.VIDEO);

                //var VideoFilename = Path.Combine(AudioFolderPath, $"{VideoName}{ext}");

                //var inputFile = new MediaFile { Filename = dlg.FileName };
                //var outputFile = new MediaFile { Filename = VideoFilename };
                var options = new ConversionOptions {
                    AudioSampleRate = AudioSampleRate.Hz22050
                };

                var engine = new Engine();



                break;

            case (int)TilesIdentifiers.Ocr:
                break;

            case (int)TilesIdentifiers.Account:
                Debug.WriteLine("Account", "Debug");
                break;

            case (int)TilesIdentifiers.document:

                CreateDialog(out dlg, ConstantsHelpers.DOCUMENTS);

                ////var path = FolderHelper.CreateFolder(ConstantsHelpers.TRANSLATIONS);

                ////if (!string.IsNullOrEmpty(dlg.FileName)) {

                ////    string FileName = Path.GetFullPath(dlg.FileName);

                ////    var sourceUri = await StorageService.UploadToAzureBlobStorage(FileName);

                ////    var targetUri = await StorageService.SaveFromdAzureBlobStorage();

                ////}

                ////break;
                break;
                //case (int)TilesIdentifiers.About:
                //    var about = new AboutWindow();
                //    about.ShowDialog();

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
}
