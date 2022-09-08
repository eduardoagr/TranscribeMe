using System.Windows;

using Config;

using Syncfusion.Licensing;

namespace TranscribeMe;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense(Constants.SYNCFUSION_KEY);
    }

}
