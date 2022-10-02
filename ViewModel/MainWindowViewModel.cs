using Config;

using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

using NAudio.Wave;

using Syncfusion.DocIO.DLS;

using TranscribeMe.Resources;
using TranscribeMe.Services;

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

    //    AudioToText
    //VideoToText
    //TranslateDocument
    //Account
    //About
    private void InitCollection() {
        Tiles = new ObservableCollection<Tile>()
        {
            new Tile(){
                IsTileActive = true,
                TileTitle = Strings.AudioToText,
                TileIdentifier = (int)TilesIdentifiers.Audio,
                TileCommand = AzureCommand,
                TileIcon = IconFont.VolumeHigh
            },
             new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.Video,
                TileTitle = Resources.Strings.VideoToText,
                TileCommand = AzureCommand,
                TileIcon = IconFont.FileVideo
            },
              new Tile(){
                IsTileActive = true,
                TileTitle = Strings.ImageToText,
                TileIdentifier = (int)TilesIdentifiers.Ocr,
                TileCommand = AzureCommand,
                TileIcon = IconFont.EyeCircle
            },
                 new Tile(){
                IsTileActive = true,
                TileTitle = Strings.TranslateDocument,
                TileIdentifier = (int)TilesIdentifiers.document,
                TileCommand = AzureCommand,
                TileIcon = IconFont.FileDocument

            },
               new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.Account,
                TileTitle = Strings.Account,
                TileCommand = AzureCommand,
                TileIcon = IconFont.Account
            },
              new Tile(){
                IsTileActive = true,
                TileIdentifier = (int)TilesIdentifiers.About,
                TileTitle = Strings.About,
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

                var DocumentName = CreateDialog(out dlg, ConstantsHelpers.DOCUMENTS);

                var path = CreateFolder(ConstantsHelpers.TRANSLATIONS);

                if (!string.IsNullOrEmpty(dlg.FileName)) {
                    var sourceUri = await storageService.UploadToAzureBlobStorage(Path.GetFullPath(dlg.FileName));

                    var targetUri = await storageService.SaveFromdAzureBlobStorage(Path.GetFullPath(dlg.FileName), path);

                    await AzureTranslationService.TranslatorAsync(sourceUri, targetUri);
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


    private async Task ConvertToTextAsync(string FilePath) {
        //Configure speech service

        var config = SpeechConfig.FromSubscription(ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

        config.EnableDictation();

        //Configure speech recognition

        var taskCompleteionSource = new TaskCompletionSource<int>();

        using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
        using var speechRecognizer = new SpeechRecognizer(config, audioConfig);
        speechRecognizer.Recognizing += SpeechRecognizer_Recognizing;
        speechRecognizer.Recognized += SpeechRecognizer_Recognized;
        speechRecognizer.SessionStarted += SpeechRecognizer_SessionStarted;
        speechRecognizer.SessionStopped += SpeechRecognizer_SessionStopped;

        await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

        Task.WaitAny(new[] { taskCompleteionSource.Task });

        await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    }

    private void SpeechRecognizer_SessionStopped(object? sender, SessionEventArgs e) {
        Tiles![0].IsTileActive = true;

        var pathToSave = CreateFolder(ConstantsHelpers.TRANSLATIONS);


        var sb = new StringBuilder();

        foreach (var item in Words) {
            sb.Append(item);
        }

        using var document = new WordDocument();

        document.EnsureMinimal();

        document.LastParagraph.AppendText(sb.ToString());

        //document.Save(filename);

        MessageBox.Show("Created");
    }

    private void SpeechRecognizer_SessionStarted(object? sender, SessionEventArgs e) {
        Tiles![0].IsTileActive = false;

        Debug.WriteLine("Started");
    }
    private void SpeechRecognizer_Recognized(object? sender, SpeechRecognitionEventArgs e) {
        if (e.Result.Reason == ResultReason.RecognizedSpeech) {
            foreach (var item in e.Result.Text) {
                Words.Add(item);
            }
        }
    }

    private void SpeechRecognizer_Recognizing(object? sender, SpeechRecognitionEventArgs e) {
    }

    enum TilesIdentifiers {
        Audio = 0,
        Video = 1,
        Ocr = 2,
        document = 3,
        Account = 4,
        About = 5,
    }
}
