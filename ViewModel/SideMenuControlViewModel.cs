using TranscribeMe.Model;
namespace TranscribeMe.ViewModel {
    public class SideMenuControlViewModel {

        ResourceDictionary dict =
            Application.LoadComponent(
            resourceLocator: new Uri(
            "/TranscribeMe;component/IconsDictionary.xaml",
            UriKind.RelativeOrAbsolute)) as ResourceDictionary;

        public List<MenuItemData> ItemsData { get; set; } = new List<MenuItemData>();

        public SideMenuControlViewModel() {
            ItemsData.Add(new MenuItemData { Icon = (System.Windows.Controls.Label)dict["AudioText"], MenuText = Lang.AudioToText });

        }
    }
}
