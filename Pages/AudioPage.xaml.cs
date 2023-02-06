

namespace TranscribeMe.Pages {

    public partial class AudioPage : Page {

        public static AudioPageViewModel Instance { get; } = new();

        public AudioPage() {
            InitializeComponent();
            DataContext = Instance;
        }

    }
}
