using Label = System.Windows.Controls.Label;

namespace TranscribeMe.Model {
    public class MenuItemData {
        public Label? Icon { get; set; }
        public bool IsItemSelected { get; set; }
        public string? MenuText { get; set; }
    }
}
