using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Helpers;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Win32;

using MvvmHelpers.Commands;

using NAudio.Wave;

using PropertyChanged;

using View;

namespace TranscribeMe.ViewModel;

[AddINotifyPropertyChangedInterface]
public class MainWindowViewModel
{
    public bool IsEnable
    {
        get; set;
    }
    public Command AzureCommand
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
        Words = new List<char>();
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

                    await ConvertToTextAsync(filePath);
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
    private async Task ConvertToTextAsync(string FilePath)
    {
        // Configure speech service
        const string KEY = "c5ba659727324477b4b34e14bec0cee6";
        const string Region = "westeurope";
        var config = SpeechConfig.FromSubscription(KEY, Region);
        config.OutputFormat = OutputFormat.Detailed;
        config.EnableDictation();


        // Configure voice
        config.SpeechSynthesisVoiceName = "en-US-AriaNeural";

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
        var sb = new StringBuilder();

        foreach (var item in Words)
        {
            sb.Append(item);
        }

        BackgroundClipboard.SetText(sb.ToString());

        if (!string.IsNullOrEmpty(BackgroundClipboard.GetText()))
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var spellWindow = new SpellCheckWindow();
                spellWindow.Show();
            });
        }
    }

    private void SpeechRecognizer_SessionStarted(object? sender, SessionEventArgs e)
    {
        IsEnable = false;
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
