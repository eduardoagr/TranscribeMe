using System.Windows.Controls;

using TranscribeMe.ViewModel;

namespace TranscribeMe.Pages {
    /// <summary>
    /// Interaction logic for TranslatePage.xaml
    /// </summary>
    public partial class TranslatePage : Page {
        public static TranslatePageViewModel Instance { get; } = new TranslatePageViewModel();
        public TranslatePage() {
            InitializeComponent();
            DataContext = Instance;
        }
    }
}
