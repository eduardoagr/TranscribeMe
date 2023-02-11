namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class ProfilePageViewModel {

        public ObservableCollection<ImageSource> ImagesCollection { get; set; }

        public AsyncCommand UpdateSaveCommand { get; set; }

        public bool IsReadOnly { get; set; } = true;

        public string? BtonContent { get; set; }

        public string UserImage { get; set; }

        public string? UserId { get; set; }

        public FirebaseObject<LocalUser>? FirebaseUserObject { get; private set; }

        #region Firebase configuraion

        private readonly FirebaseServices FireService;

        private readonly string? DatabaseUrl;

        #endregion

        public ProfilePageViewModel(string userId, string databaseUrl) {

            ImagesCollection = AvatarHelper.GetAvatars();

            UserImage = "/Images/Placeholder.png";

            DatabaseUrl = databaseUrl;
            UserId = userId;
            FireService = new FirebaseServices(DatabaseUrl);
            LoadDataFromFirebase();
            BtonContent = Lang.Edit;
            UpdateSaveCommand = new AsyncCommand(UpdateSaveCommandActionAsync);
            FireService = new FirebaseServices(DatabaseUrl);
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
            if (!string.IsNullOrEmpty(UserId)) {
                FirebaseUserObject = await FireService.GetAsync("Users", UserId!);
            }
        }

        private async Task UpdateDataFromFirebase() {

            if (FirebaseUserObject != null) {

                var key = FirebaseUserObject.Key;

                FirebaseUserObject.Object.Username = FirebaseUserObject.Object.Username;
                FirebaseUserObject.Object.PhotoUrl = UserImage;

                await FireService.UpdateAsync("Users", key, FirebaseUserObject);
            }
        }
    }
}
