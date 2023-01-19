using Microsoft.Toolkit.Uwp.Notifications;

namespace TranscribeMe.Helpers {
    public class ToastHelper {
        public static void LaunchToastNotification(string FilePath) {
            ToastNotificationManagerCompat.OnActivated += toastArgs => {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                Application.Current.Dispatcher.Invoke(delegate {
                    if (args.Count > 0) {
                        Process p = new();
                        p.StartInfo.FileName = FilePath;
                        p.StartInfo.UseShellExecute = true;
                        p.Start();
                    }

                    return Task.CompletedTask;
                });
            };

            new ToastContentBuilder()
            .AddText(Lang.ToastMsg1)
            .AddText(Lang.ToastMsg2)
            .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Images\Document.png"), UriKind.Absolute), ToastGenericAppLogoCrop.Circle)
            .AddButton(new ToastButton()
                .SetContent("Open document")
                .AddArgument("action", "openDec"))
            .Show();
        }
    }
}
