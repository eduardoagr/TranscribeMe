using TranscribeMe.ViewModel;

namespace TranscribeMe.View {
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window {
        public AboutWindow() {
            InitializeComponent();

            DataContext = new AboutWindowViewModel();
        }
    }
}
