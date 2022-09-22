using Syncfusion.DocIO.DLS;

namespace TranscribeMe.ViewModel;

[AddINotifyPropertyChangedInterface]
public class MainWindowViewModel
{

    public Command ExitCommand
    {
        get; set;
    }

    public Command AzureCommand
    {
        get; set;
    }

    public ObservableCollection<Tile>? Tiles
    {
        get; set;
    }
    public List<char> Words
    {
        get; set;
    }


    public MainWindowViewModel()
    {
        AzureCommand = new Command(AzureActionAsync);
        ExitCommand = new Command(ExiAction);
        InitCollection();
        Words = new List<char>();

        Tiles![0].IsTileActive = false;
    }

    private void InitCollection()
    {

        Tiles = new ObservableCollection<Tile>()
        {
            new Tile()
            {
                IsTileActive = true,
                TileTitle = "Audio to text",
                TileIdentifier = "1",
                TileCommand = AzureCommand,
                TileColor = new SolidColorBrush(Color.FromRgb(242, 80, 34)),
                TileIcon = IconFont.VolumeHigh
            },
             new Tile()
            {
                IsTileActive = true,
                TileTitle = "Translate document",
                TileIdentifier = "2",
                TileCommand = AzureCommand,
                TileColor = new SolidColorBrush(Color.FromRgb(127, 186, 0)),
                TileIcon = IconFont.FileDocument
            },
              new Tile()
            {
                IsTileActive = false,
                TileIdentifier = "3",
                TileTitle = "Video to text" ,
                TileCommand = AzureCommand,
                TileColor = new SolidColorBrush(Color.FromRgb(0, 164, 239)),
                TileIcon = IconFont.FileVideo
            },
               new Tile()
            {
                IsTileActive = false,
                TileIdentifier = "4",
                TileTitle = "Account",
                TileCommand = AzureCommand,
                TileColor = new SolidColorBrush(Color.FromRgb(255, 185, 0)),
                TileIcon = IconFont.Account
            }
        };
    }

    private void ExiAction()
    {
        Application.Current.Shutdown();
    }

    private async void AzureActionAsync(object obj)
    {
        var str = obj as string;

        if (!string.IsNullOrEmpty(str))
        {


            switch (str)
            {
                case "1":
                    const string ext = ".wav";
                    var dlg = new OpenFileDialog
                    {
                        DefaultExt = ".mp3",
                        Filter = "Audio files (.mp3)|*.mp3"
                    };

                    var res = dlg.ShowDialog();

                    if (res! == true)
                    {
                        var AudioName = Path.GetFileNameWithoutExtension(dlg.SafeFileName);
                        var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
                        var FoderName = Path.Combine(projectPath!, "Audios");
                        var filePath = Path.Combine(FoderName, $"{AudioName}{ext}");

                        using var mp3 = new Mp3FileReader(dlg.FileName);
                        using var ws = WaveFormatConversionStream.CreatePcmStream(mp3);
                        WaveFileWriter.CreateWaveFile(filePath, ws);

                        await ConvertToTextAsync(filePath);
                    }
                    break;
            }
        }
    }

    private async Task ConvertToTextAsync(string FilePath)
    {
        // Configure speech service

        var config = SpeechConfig.FromSubscription(Config.Constants.AZURE_KEY, Config.Constants.AZURE_REGION);

        // Configure speech recognition

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

    private void SpeechRecognizer_SessionStopped(object? sender, SessionEventArgs e)
    {

        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var folder = Path.Combine(path, "Transcribed");
        var filename = Path.Combine(folder, "Hello.docx");
        Directory.CreateDirectory(folder);

        var sb = new StringBuilder();

        foreach (var item in Words)
        {
            sb.Append(item);
        }

        using var document = new WordDocument();

        document.EnsureMinimal();

        document.LastParagraph.AppendText(sb.ToString());

        document.Save(filename);

        MessageBox.Show("Created");
    }

    private void SpeechRecognizer_SessionStarted(object? sender, SessionEventArgs e)
    {


        Debug.WriteLine("Started");
    }
    private void SpeechRecognizer_Recognized(object? sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            foreach (var item in e.Result.Text)
            {
                Words.Add(item);
            }
        }
    }

    private void SpeechRecognizer_Recognizing(object? sender, SpeechRecognitionEventArgs e)
    {
    }
}
