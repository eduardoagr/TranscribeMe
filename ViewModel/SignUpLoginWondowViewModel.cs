namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class SignUpLoginWondowViewModel {

        private CurrentCity? _currentCity { get; set; }

        private GeoServices _geoServices { get; set; }

        private readonly IFirebaseAuthClient _firebaseAuthClient;

        private bool isShowingRegister { get; set; }

        public AsyncCommand RegisterCommand { get; set; }

        public AsyncCommand LoginCommand { get; set; }

        public Command SwitchViewsCommand { get; set; }

        public LocalUser User { get; set; }

        public Visibility RegisterVis { get; set; }

        public Visibility LoginVis { get; set; }

        public SignUpLoginWondowViewModel(IFirebaseAuthClient firebaseAuthClient) {
            _firebaseAuthClient = firebaseAuthClient;
            _geoServices = new GeoServices();
            User = new LocalUser();
            User.PropertyChanged += (sender, args) => RegisterCommand?
            .RaiseCanExecuteChanged();

            User.PropertyChanged += (sender, args) => LoginCommand?
            .RaiseCanExecuteChanged();

            RegisterCommand = new AsyncCommand(RegisterActionAsync, CanRegster);
            LoginCommand = new AsyncCommand(LoginAction, CanLogin);
            SwitchViewsCommand = new Command(SwitchViews);

            //File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userdata.json"));

            LoginVis = Visibility.Visible;
            Application.Current.Windows[0].Title = "Login";
            RegisterVis = Visibility.Collapsed;

        }



        private async Task RegisterActionAsync() {

            try {
                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync
                    (User.Email, User.Password, User.Username);
                if (result.User != null) {

                    await GetGeoData();

                    var firebase = new FirebaseClient(
                        "https://transcribed-c69c8-default-rtdb.europe-west1.firebasedatabase.app/");

                    var newUser =
                        await firebase.Child("Users").
                        PostAsync(
                            new LocalUser(_currentCity?.address?.county,
                            result.User.Uid,
                            result.User.Info.Email,
                            result.User.Info.DisplayName,
                            _currentCity?.address?.city,
                            User.hasPaid,
                            new DateTime(default),
                            new DateTime(default),
                            User.isActive));

                    // Save user data in a local file
                    string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userdata.json");

                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Application.Current.Windows[0].Close();

                }
            } catch (FirebaseAuthException ex) {
                await ExceptionAsync(ex);
            }
        }

        private async Task LoginAction() {

            try {
                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(
                    User.Email, User.Password);
                if (result.User != null && !string.IsNullOrEmpty(result.User.Uid)) {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Application.Current.Windows[0].Close();
                }
            } catch (FirebaseAuthException ex) {
                await ExceptionAsync(ex);

            }
        }

        private static async Task ExceptionAsync(FirebaseAuthException ex) {
            var message = ex.Message;
            var startIndex = message.IndexOf("Response: ") + "Response: ".Length;
            var endIndex = message.IndexOf("\n\nReason");
            var jsonString = message[startIndex..endIndex];
            var error = JsonSerializer.Deserialize<FirebaseResponse>(jsonString);
            switch (error!.error.message) {

                case "INVALID_PASSWORD":
                    await ShowContentDialogAsync(Lang.INVALID_PASSWORD, "OK", "error");
                    break;
                case "EMAIL_NOT_FOUND":
                    await ShowContentDialogAsync(Lang.EMAIL_NOT_FOUND, "OK", "error");
                    break;

                case "EMAIL_EXISTS":
                    await ShowContentDialogAsync(Lang.EMAIL_EXISTS, "OK", "error");
                    break;

                case "INVALID_EMAIL":
                    await ShowContentDialogAsync(Lang.INVALID_EMAIL, "OK", "error");
                    break;

            }
        }

        private static async Task ShowContentDialogAsync(string msg, string closeBtn, string title) {

            ContentDialog dialog = new() {
                Title = title,
                Content = msg,
                CornerRadius = new CornerRadius(10),
                CloseButtonText = closeBtn,

            };

            await dialog.ShowAsync();

        }

        private async Task GetGeoData() {
            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed) {
                var locator = new Geolocator();

                var pos = await locator.GetGeopositionAsync();

                var userCity = await _geoServices.GetLocation(pos.Coordinate.Latitude, pos.Coordinate.Longitude);

                _currentCity = userCity;
            }
        }

        private bool CanRegster(object arg) {
            if (!string.IsNullOrEmpty(User.Email)
                && !string.IsNullOrEmpty(User.Password)
                && !string.IsNullOrEmpty(User.Username)
                && !string.IsNullOrEmpty(User.Confirm)
                && User.Confirm == User.Password
                && User.Password.Length == 6
                && new EmailAddressAttribute().IsValid(User.Email)) {
                return true;
            }
            return false;
        }

        private bool CanLogin(object arg) {

            if (!string.IsNullOrEmpty(User.Email)
                && !string.IsNullOrEmpty(User.Password)
                && new EmailAddressAttribute().IsValid(User.Email)) {
                return true;
            }
            return false;
        }

        private void SwitchViews() {

            isShowingRegister = !isShowingRegister;
            if (isShowingRegister) {
                Application.Current.Windows[0].Title = "Register";
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            } else {
                Application.Current.Windows[0].Title = "Login";
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

    }
}
