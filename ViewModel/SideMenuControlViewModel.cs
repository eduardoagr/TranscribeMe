using System.Windows.Controls;

using TranscribeMe.Model;
namespace TranscribeMe.ViewModel {
    public class SideMenuControlViewModel {

        ResourceDictionary dict =
            Application.LoadComponent(new Uri(
            "/IconsDictionary.xaml",
            UriKind.RelativeOrAbsolute)) as ResourceDictionary;

        public List<MenuItemData> ItemsData { get; set; } = new List<MenuItemData>();

        public SideMenuControlViewModel() {
            ItemsData.Add(new MenuItemData { Icon = (Label)dict["AudioText"], MenuText = Lang.AudioToText });

        }
    }
}
