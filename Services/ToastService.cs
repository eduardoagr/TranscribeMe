using Microsoft.Toolkit.Uwp.Notifications;

namespace TranscribeMe.Services {
    public class ToastService {

        public static void LaunchToastNotification(string FilePath) {

            ToastNotificationManagerCompat.OnActivated += toastArgs => {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                var startInfo = new ProcessStartInfo {
                    FileName = "WINWORD.EXE",
                    Arguments = FilePath
                };
                startInfo.Arguments = "doc";
                Process.Start(startInfo);
            };

            new ToastContentBuilder()
                .AddText(Lang.ToastMsg1)
                .AddText(Lang.ToastMsg2)
                .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Images\Word.png"), UriKind.Absolute), ToastGenericAppLogoCrop.Circle)
                .AddButton(new ToastButton()
                    .SetContent("Open document")
                    .AddArgument("action", "openDec")
                    .AddArgument("doc", FilePath))
                .Show();
        }
    }
}
