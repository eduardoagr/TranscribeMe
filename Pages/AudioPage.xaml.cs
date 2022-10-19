using System.Windows.Controls;

using TranscribeMe.ViewModel;

namespace TranscribeMe.Pages {

    public partial class AudioPage : Page {
        public AudioPage() {
            InitializeComponent();
            DataContext = new AudioPageViewModel();
        }
    }
}
