using System.Diagnostics;
using System.IO;
using System.Windows;

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
        AzureCommand = new Command(AzureAction);
    }

    private void AzureAction(object obj)
    {
        var str = obj as string;

        if (string.IsNullOrEmpty(str))
        {
            return;
        }

        switch (str)
        {
            case "Audio":
                var dlg = new OpenFileDialog
                {
                    DefaultExt = ".mp3",
                    Filter = "Audio files (.mp3)|*.mp3"
                };

                var res = dlg.ShowDialog();

                if (res! == true)
                {
                    var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
                    var FoderName = Path.Combine(projectPath!, "Audios");
                    Directory.CreateDirectory(FoderName);

                    using (var mp3 = new Mp3FileReader(dlg.FileName))
                    {
                        using (var ws = WaveFormatConversionStream.CreatePcmStream(mp3))
                        {
                            WaveFileWriter.CreateWaveFile(FoderName, ws);
                        }

                    }
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
}
