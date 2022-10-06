
using Microsoft.Toolkit.Uwp.Notifications;

namespace TranscribeMe.Services {
    public class ToastService {

        public static void LaunchToastNotification(string FilePath, string ext) {

            ToastNotificationManagerCompat.OnActivated += toastArgs => {
                // Obtain the arguments from the notification
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                // Need to dispatch to UI thread if performing UI operations
                Application.Current.Dispatcher.Invoke(async delegate {
                    // TODO: Show the corresponding content
                    if (ext == ".pdf") {
                        MessageBox.Show(".pdf");
                    } else {
                        //
                    }
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
