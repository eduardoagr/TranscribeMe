using System.Windows;

using Syncfusion.Licensing;

namespace TranscribeMe;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string KEY = "NzA0MTkxQDMyMzAyZTMyMmUzMGZKeXA0LzhmYnlrQ3lnOWkwZ0I1aFlUQTd2Qm4xN08rMTFnY051Z0dTOFU9";
    public App()
    {
        //Register Syncfusion license
        SyncfusionLicenseProvider.RegisterLicense(KEY);
    }

}
