using TranscribeMe.View;

namespace TranscribeMe.ViewModel;

public class MainWindowViewModel {
    public Command ExitCommand { get; set; }

    public Command AzureCommand { get; set; }

    public ObservableCollection<Tile>? Tiles { get; set; }

    public List<char> Words { get; set; }

    public AzureStorageService StorageService { get; set; }

    public AzureTranscriptionService AzureTranscription { get; set; }

    public MainWindowViewModel() {
        AzureCommand = new Command(AzureActionAsync);
        ExitCommand = new Command(ExiAction);
        StorageService = new AzureStorageService();
        AzureTranscription = new AzureTranscriptionService();
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
        var AudioFolderPath = CreateFolderService.CreateFolder(ConstantsHelpers.AUDIO);
        const string ext = ".wav";

        switch (obj) {
            case (int)TilesIdentifiers.Audio:

                var AudioName = CreateDialog(out dlg, ConstantsHelpers.AUDIO);

                var Audiofilename = Path.Combine(AudioFolderPath, $"{AudioName}{ext}");

                Converter(dlg, Audiofilename, out _, out _);

                await AzureTranscription.ConvertToTextAsync(Audiofilename, AudioName!, 0, Tiles!, Words);

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

                    await AzureTranscription.ConvertToTextAsync(VideoFilename, VideoName!, 1, Tiles!, Words);
                }

                break;

            case (int)TilesIdentifiers.Ocr:
                break;

            case (int)TilesIdentifiers.Account:
                Debug.WriteLine("Account", "Debug");
                break;

            case (int)TilesIdentifiers.document:

                CreateDialog(out dlg, ConstantsHelpers.DOCUMENTS);

                var path = CreateFolderService.CreateFolder(ConstantsHelpers.TRANSLATIONS);

                if (!string.IsNullOrEmpty(dlg.FileName)) {

                    var sourceUri = await StorageService.UploadToAzureBlobStorage(Path.GetFullPath(dlg.FileName));

                    var targetUri = await StorageService.SaveFromdAzureBlobStorage(Path.GetFullPath(dlg.FileName));

                    await AzureTranslationService.TranslatorAsync(sourceUri, targetUri, 3, Tiles!);

                    var PathToSave = Path.Combine(path, dlg.FileName);

                    //using var client = new HttpClient();
                    //using var s = await client.GetStreamAsync(targetUri);
                    //using var fs = new FileStream(PathToSave, FileMode.OpenOrCreate);
                    //await s.CopyToAsync(fs);
                }

                break;

            case (int)TilesIdentifiers.About:
                var about = new AboutWindow();
                about.ShowDialog();

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
}
