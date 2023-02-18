using ModernWpf;

using Color = System.Windows.Media.Color;

namespace TranscribeMe.View.Dialogs {

    public partial class VideoPreviewDialog : Window {

        AxWindowsMediaPlayer? player = null;

        public VideoPreviewDialog(string filepath) {
            InitializeComponent();

            player = host.Child as AxWindowsMediaPlayer;
            host.Child = player;
            player.uiMode = "full";
            player.URL = filepath;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            player.Dispose();
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            // Get the current theme
            var currentTheme = ThemeManager.Current.ActualApplicationTheme;

            // Update the window background based on the theme
            if (currentTheme == ApplicationTheme.Dark) {
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            } else {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
    }
}