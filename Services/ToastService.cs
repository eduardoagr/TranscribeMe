using Microsoft.Toolkit.Uwp.Notifications;

namespace TranscribeMe.Services {
    public class ToastService {
        public static void CreateAndShowPrompt() {
            new ToastContentBuilder()
             .AddArgument("action", "viewConversation")
             .AddArgument("conversationId", 9813)
             .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath(@"Images\Word.png"), UriKind.Absolute), ToastGenericAppLogoCrop.Circle)
             .AddText(Lang.ToastMsg1)
             .AddText(Lang.ToastMsg2)
             .Show(); //
        }
    }
}
