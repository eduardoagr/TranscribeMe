using Config;

using Syncfusion.Licensing;

namespace TranscribeMe;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public App() {
        SyncfusionLicenseProvider.RegisterLicense(ConstantsHelpers.SYNCFUSION_KEY);
    }

}
