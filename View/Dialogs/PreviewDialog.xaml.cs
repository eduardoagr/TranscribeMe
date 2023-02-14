using AxWMPLib;

using System.Windows.Forms.Integration;

namespace TranscribeMe.View.Dialogs {

    public partial class PreviewDialog : Window {
        public PreviewDialog(string filepath) {
            InitializeComponent();

            var player = new AxWindowsMediaPlayer();
            player.CreateControl();
            var host = (WindowsFormsHost)FindName("host");
            host.Child = player;
            player.URL = filepath;
            Closing += PreviewDialog_Closing;
        }

        private void PreviewDialog_Closing(object? sender, EventArgs e) {
            player.close();
        }
    }
}