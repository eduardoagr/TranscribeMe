using Firebase.Auth;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using TranscribeMe.View;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class SignUpLoginWondowViewModel {

        //This is the user from firebse
        private User? _user { get; set; }

        private readonly IFirebaseAuthClient _firebaseAuthClient;

        private bool isShowingRegister = false;

        public AsyncCommand RegisterCommand { get; set; }

        public Command LoginCommand { get; set; }

        public Command SwitchViewsCommand { get; set; }

        public LocalUser User { get; set; }

        public Visibility RegisterVis { get; set; }

        public Visibility LoginVis { get; set; }

        public SignUpLoginWondowViewModel(IFirebaseAuthClient firebaseAuthClient) {
            _firebaseAuthClient = firebaseAuthClient;
            User = new LocalUser();
            User.PropertyChanged += (sender, args) => RegisterCommand?.RaiseCanExecuteChanged();
            RegisterCommand = new AsyncCommand(RegisterActionAsync, CanRegster);
            LoginCommand = new Command(LoginAction, CanLogin);
            LoginWithSavedData();
            SwitchViewsCommand = new Command(SwitchViews);


            LoginVis = Visibility.Visible;
            RegisterVis = Visibility.Collapsed;
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

        private async Task RegisterActionAsync() {

            try {
                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(User.Email, User.Password, User.Username);
                if (result.User != null) {
                    _user = result.User;
                    // Save user data in a local file
                    string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userdata.json");
                    File.WriteAllText(userDataFile, JsonSerializer.Serialize(_user));
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                }
            } catch (FirebaseAuthException ex) {
                var message = ex.Message;
                var startIndex = message.IndexOf("Response: ") + "Response: ".Length;
                var endIndex = message.IndexOf("\n\nReason");
                var jsonString = message[startIndex..endIndex];
                var error = JsonSerializer.Deserialize<FirebaseResponse>(jsonString);
                switch (error!.error.message) {

                    case "EMAIL_EXISTS":
                        MessageBox.Show(Lang.EMAIL_EXISTS);
                        break;

                    case "INVALID_EMAIL":
                        MessageBox.Show(Lang.INVALID_EMAIL);
                        break;

                }
            }
        }

        private static void LoginWithSavedData() {
            string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userdata.json");
            if (File.Exists(userDataFile)) {
                var savedUser = JsonSerializer.Deserialize<FireUser>(File.ReadAllText(userDataFile));
                if (!string.IsNullOrEmpty(savedUser!.Uid)) {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                }
            }
        }


        private void LoginAction(object obj) {

        }

        private bool CanLogin(object arg) {

            if (string.IsNullOrEmpty(User.Password)
                && !string.IsNullOrEmpty(User.Username)) {
                return true;
            }
            return false;
        }

        private void SwitchViews() {

            isShowingRegister = !isShowingRegister;
            if (isShowingRegister) {
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            } else {
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }
    }
}
