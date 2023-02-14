namespace TranscribeMe.Helpers {
    public class BullePointHelper {

        public static ObservableCollection<string> GetBulletPoints() {

            return new ObservableCollection<string>() {

                {"Azure.AI.Translation.Document"},
                {"Azure.Core" },
                {"Azure.Storage.Blobs" },
                {"FirebaseAuthentication.net"},
                {"FirebaseDatabase.net"},
                {"MahApps.Metro"},
                {"PropertyChanged.Fody"},
                {"Syncfusion.Pdf.Net.Core"},
                {"Syncfusion.DocIO.Wpf" },
                {"Syncfusion.Licensing" },
                {"Refractored.MvvmHelpers"},
                {"NAudio.Lame"},
                {"NAudio"},
                {"ModernWpfUI.MahApps"},
                {"ModernWpfUI"},
                {"Microsoft.Xaml.Behaviors.Wpf" },
                {"Microsoft.VisualStudio.Shell.Interop" },
                {"Microsoft.Toolkit.Uwp.Notifications"},
                {"Microsoft.Extensions.Hosting"},
                {"Microsoft.CognitiveServices.Speech"},
                {"Microsoft.Azure.CognitiveServices.Vision.ComputerVision"},

            };
        }
    }
}
