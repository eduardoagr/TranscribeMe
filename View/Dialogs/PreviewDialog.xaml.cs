
namespace TranscribeMe.View.Dialogs {

    public partial class PreviewDialog : ContentDialog {


        public PreviewDialog(string filepath) {
            InitializeComponent();

            var player = new AxWindowsMediaPlayer();
            player.CreateControl();
            var host = (WindowsFormsHost)FindName("host");
            host.Child = player;
            player.URL = filepath;
            Closing += PreviewDialog_Closing;
            Unloaded += PreviewDialog_Unloaded;
        }

        private void PreviewDialog_Unloaded(object sender, RoutedEventArgs e) {
            player.Dispose();
        }

        private void PreviewDialog_Closing(object? sender, EventArgs e) {
            player.close();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }
    }
}