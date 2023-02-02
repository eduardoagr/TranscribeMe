namespace TranscribeMe.View {

    public partial class SignUpLoginWondow : Window {

        public SignUpLoginWondowViewModel Instance { get; set; }

        public SignUpLoginWondow(IFirebaseAuthClient firebaseAuthClient) {
            Instance = new SignUpLoginWondowViewModel(firebaseAuthClient);
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
