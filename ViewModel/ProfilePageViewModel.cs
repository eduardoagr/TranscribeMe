namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class ProfilePageViewModel {

        private const string DatabaseURL = "https://transcribed-c69c8-default-rtdb.europe-west1.firebasedatabase.app/";

        public ObservableCollection<FirebaseObject<LocalUser>>? UserData { get; set; }

        public bool IsReadOnly { get; set; } = true;

        public string? BtonContent { get; set; }

        public AsyncCommand UpdateSaveCommand { get; set; }

        public string UserId { get; set; }

        public LocalUser localUser { get; set; }

        public FirebaseClient FireClient { get; set; }

        public ProfilePageViewModel(string userId) {

            UserId = userId;
            FireClient = new FirebaseClient(DatabaseURL);
            LoadDataFromFirebase();
            BtonContent = Lang.Edit;
            UpdateSaveCommand = new AsyncCommand(UpdateSaveCommandActionAsync);
        }

        public ProfilePageViewModel() {

        }


        private async Task UpdateSaveCommandActionAsync() {
            if (BtonContent!.Equals(Lang.Edit)) {
                IsReadOnly = false;
                BtonContent = Lang.Update;
            } else {
                await UpdateDataFromFirebase();
                IsReadOnly = true;
                BtonContent = Lang.Edit;
            }
        }

        private async void LoadDataFromFirebase() {

            UserData = new ObservableCollection<FirebaseObject<LocalUser>>();

            var users = await ReadFromFirebaseAsync
                ("Users", FireClient);

            UserData.Clear();
            foreach (var item in users) {
                if (item.Object.Id == UserId) {
                    localUser = item.Object;
                }

            }

        }

        private async Task UpdateDataFromFirebase() {

            var firebaseObjects = await ReadFromFirebaseAsync
            ("Users", FireClient);

            var userToUpdate = firebaseObjects.FirstOrDefault(
                x => x.Object.Id == UserId);

            userToUpdate!.Object.Username = localUser.Username;
            userToUpdate!.Object.FirstName = localUser.FirstName;
            userToUpdate!.Object.LastName = localUser.LastName;

            await UpdateFirebaseAsync(userToUpdate, "Users", FireClient);
        }

        private static async Task<IReadOnlyCollection<FirebaseObject<LocalUser>>> ReadFromFirebaseAsync
            (string Node, FirebaseClient firebaseClient) {

            var FireUsers = await firebaseClient
               .Child(Node)
               .OnceAsync<LocalUser>();

            return FireUsers;

        }

        private static async Task UpdateFirebaseAsync(FirebaseObject<LocalUser>? userToUpdate,
         string Node, FirebaseClient firebaseClient) {

            await firebaseClient.Child(Node)
                .Child(userToUpdate!.Key)
                .PutAsync(userToUpdate.Object);
        }
    }
}
