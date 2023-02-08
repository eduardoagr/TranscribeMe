namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class SignUpLoginWondowViewModel {

        private CurrentCity? _currentCity { get; set; }

        private GeoServices _geoServices { get; set; }

        private readonly IFirebaseAuthClient _firebaseAuthClient;

        private bool isShowingRegister { get; set; }

        public bool isBusy { get; set; } = false;

        public AsyncCommand RegisterCommand { get; set; }

        public AsyncCommand LoginCommand { get; set; }

        public Command SwitchViewsCommand { get; set; }

        public LocalUser User { get; set; }

        public Visibility RegisterVis { get; set; }

        public Visibility LoginVis { get; set; }

        public Visibility isButtonVisible { get; set; }

        public Visibility IsLoginVis { get; set; }

        public SignUpLoginWondowViewModel(IFirebaseAuthClient firebaseAuthClient) {

            isButtonVisible = Visibility.Visible;
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
            IsLoginVis = Visibility.Visible;
            Application.Current.Windows[0].Title = Lang.Login;
            RegisterVis = Visibility.Collapsed;

        }

        private async Task RegisterActionAsync() {

            try {
                isButtonVisible = Visibility.Collapsed;
                isBusy = true;
                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync
                    (User.Email, User.Password, User.Username);
                if (result.User != null) {

                    await GetGeoData();

                    var firebase = new FirebaseClient(
                        "https://transcribed-c69c8-default-rtdb.europe-west1.firebasedatabase.app/");

                    var newUser =
                        await firebase.Child("Users").
                        PostAsync(
                            new LocalUser(result.User.Uid,
                            result.User.Info.DisplayName,
                            User.FirstName!,
                            User.LastName,
                            User.PhotoUrl,
                            result.User.Info.Email,
                            _currentCity?.address!.country,
                            _currentCity!.address!.city,
                            User.HasPaid, User.IsActive,
                            User.DateOfBirth,
                            new DateTime(),
                            new DateTime()));

                    // Save user data in a local file
                    string userDataFile = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                            "userdata.json");


                    File.WriteAllText(userDataFile, JsonSerializer.Serialize(newUser));


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
                isButtonVisible = Visibility.Collapsed;
                isBusy = true;
                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(
                    User.Email, User.Password);
                if (result.User != null && !string.IsNullOrEmpty(result.User.Uid)) {

                    MainWindow mainWindow = new() {
                        DataContext = new MainWindowViewModel(result.User.Uid)
                    };
                    mainWindow.Show();
                    Application.Current.Windows[0].Close();
                }
            } catch (FirebaseAuthException ex) {
                await ExceptionAsync(ex);



            }
        }

        private async Task ExceptionAsync(FirebaseAuthException ex) {
            var message = ex.Message;
            var startIndex = message.IndexOf("Response: ") + "Response: ".Length;
            var endIndex = message.IndexOf("\n\nReason");
            var jsonString = message[startIndex..endIndex];
            var error = JsonSerializer.Deserialize<FirebaseResponse>(jsonString);
            switch (error!.error.message) {

                case "INVALID_PASSWORD":
                    await ShowContentDialogAsync(Lang.INVALID_PASSWORD, "OK");
                    break;
                case "EMAIL_NOT_FOUND":
                    await ShowContentDialogAsync(Lang.EMAIL_NOT_FOUND, "OK");
                    break;

                case "EMAIL_EXISTS":
                    await ShowContentDialogAsync(Lang.EMAIL_EXISTS, "OK");
                    break;

                case "INVALID_EMAIL":
                    await ShowContentDialogAsync(Lang.INVALID_EMAIL, "OK");
                    break;

            }
        }

        private async Task ShowContentDialogAsync(string msg, string closeBtn) {

            isBusy = false;
            isButtonVisible = Visibility.Visible;

            var customDialog = new ErrorDialog {
                DataContext = new ErrorDialogViewMoel(Lang.ErrorDialog, msg, closeBtn),
            };
            customDialog.PrimaryBtn.Click
                += (sender, args) => { customDialog.Hide(); };

            await customDialog.ShowAsync();

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
                Application.Current.Windows[0].Title = Lang.CreateAccount;
                IsLoginVis = Visibility.Collapsed;
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            } else {
                Application.Current.Windows[0].Title = Lang.Login;
                IsLoginVis = Visibility.Visible;
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

    }
}
