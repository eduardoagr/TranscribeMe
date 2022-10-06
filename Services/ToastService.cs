
using Microsoft.Toolkit.Uwp.Notifications;

namespace TranscribeMe.Services {
    public class ToastService {

        public static void LaunchToastNotification(string FilePath) {

            ToastNotificationManagerCompat.OnActivated += toastArgs => {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                Application.Current.Dispatcher.Invoke(delegate {
                    Process wordProcess = new Process();
                    wordProcess.StartInfo.FileName = FilePath;
                    wordProcess.StartInfo.UseShellExecute = true;
                    wordProcess.Start();
                    return Task.CompletedTask;
                });
            };

            new ToastContentBuilder()
            .AddText(Lang.ToastMsg1)
            .AddText(Lang.ToastMsg2)
            .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Images\Word.png"), UriKind.Absolute), ToastGenericAppLogoCrop.Circle)
            .AddButton(new ToastButton()
                .SetContent("Open document")
                .AddArgument("action", "openDec"))
            .Show();
        }
    }
}
