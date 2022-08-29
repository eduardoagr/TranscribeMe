using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;

using MvvmHelpers.Commands;

using NAudio.Wave;

using PropertyChanged;

namespace TranscribeMe.ViewModels;

[AddINotifyPropertyChangedInterface]
public class MainWindowViewModel
{
    public Visibility IsVisible { get; set; } = Visibility.Hidden;

    public string Title { get; set; } = "Your text was copied";

    public string Sub
    {
        get; set;
    } = "Press WIN+V to access the clipboard " +
        "\n Or CTRL+V to paste right away";

    public string? Text
    {
        get; set;
    }

    public Command CopyCommand
    {
        get; set;
    }

    public Command AzureCommand
    {
        get; set;
    }

    public MainWindowViewModel()
    {
        CopyCommand = new Command(CopyAction);
        AzureCommand = new Command(AzureActionAsync);
    }

    private async void AzureActionAsync(object obj)
    {
        var str = obj as string;

        if (string.IsNullOrEmpty(str))
        {
            return;
        }

        switch (str)
        {
            case "Audio":
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

                    await ConvertToTetAsync(filePath);
                }

                break;
            case "Document":
                Debug.WriteLine("deddew");
                break;
            default:
                MessageBox.Show("An error has occurred");
                break;
        }
    }

    private void TranscribeaMeAction(object obj)
    {
        //TODO: call transcription Api
    }

    private void CopyAction()
    {
        new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationId", 9813)
        .AddText(Title)
        .AddText(Sub)
        .Show();
        if (!string.IsNullOrEmpty(Text))
        {
            Clipboard.SetText(Text);
        }
    }

    private async Task ConvertToTetAsync(string FilePath)
    {
        // Configure speech service
        const string KEY = "3a04293bb4de464a915dc1c0bf3f4283";
        const string Region = "westeurope";
        var config = SpeechConfig.FromSubscription(KEY, Region);
        MessageBox.Show("Ready to use speech service in " + config.Region);

        // Configure voice
        config.SpeechSynthesisVoiceName = "en-US-AriaNeural";

        // Configure speech recognition

        using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
        using var speechRecognizer = new SpeechRecognizer(config, audioConfig);
        speechRecognizer.Recognizing += SpeechRecognizer_Recognizing;
        speechRecognizer.Recognized += SpeechRecognizer_Recognized;
        speechRecognizer.SessionStarted += SpeechRecognizer_SessionStarted;
        speechRecognizer.SessionStopped += SpeechRecognizer_SessionStopped;

        await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

        do
        {
        } while (string.IsNullOrEmpty(Text));

        await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);



    }

    private void SpeechRecognizer_SessionStopped(object? sender, SessionEventArgs e)
    {

        Debug.WriteLine("Stopped");

    }

    private void SpeechRecognizer_SessionStarted(object? sender, SessionEventArgs e)
    {
        Debug.WriteLine("Started");
    }

    private void SpeechRecognizer_Recognized(object? sender, SpeechRecognitionEventArgs e)
    {
        var res = e.Result;
        if (res.Reason == ResultReason.RecognizedSpeech)
        {
            Text = res.Text;
            IsVisible = Visibility.Visible;
        }
    }

    private void SpeechRecognizer_Recognizing(object? sender, SpeechRecognitionEventArgs e)
    {
        Debug.WriteLine("Recogizing: " + e.Result.Text);
    }
}
