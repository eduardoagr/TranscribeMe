using Syncfusion.Licensing;

using System.Globalization;

namespace TranscribeMe;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public App() {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        SyncfusionLicenseProvider.RegisterLicense(ConstantsHelpers.SYNCFUSION_KEY);
    }

}
