namespace TranscribeMe.View {

    public partial class SignUpLoginWondow : Window {

        public SignUpLoginWondowViewModel Instance { get; set; }

        public SignUpLoginWondow(FirebaseAuthConfig config, string DatabaeURL) {
            Instance = new SignUpLoginWondowViewModel(config, DatabaeURL);
            InitializeComponent();
            DataContext = Instance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            MouseDown += delegate {
                DragMove();
            };
        }
    }
}
