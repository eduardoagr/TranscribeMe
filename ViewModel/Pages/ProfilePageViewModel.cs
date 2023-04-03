namespace TranscribeMe.ViewModel.Pages {

    [AddINotifyPropertyChangedInterface]
    public class ProfilePageViewModel {

        public ObservableCollection<string> ImagesCollection { get; set; }

        public AsyncCommand UpdateSaveCommand { get; set; }

        public bool IsReadOnly { get; set; } = true;

        public bool IsGridViewEnable { get; set; }

        public string? BtonContent { get; set; }

        public string UserImage { get; set; }

        public string? UserId { get; set; }

        public string? SelectedItem { get; set; }

        public AzureStorageService AzureStorageService { get; set; }

        public FirebaseObject<LocalUser>? FirebaseUserObject { get; private set; }

        private const string Node = "Users";

        #region Firebase configuraion

        private readonly FirebaseServices FireService;

        private readonly string? DatabaseUrl;

        #endregion

        public ProfilePageViewModel(string userId, string databaseUrl) {

            ImagesCollection = AvatarHelper.GetAvatars();

            UserImage = "/Images/Placeholder.png";


            AzureStorageService = new AzureStorageService();
            DatabaseUrl = databaseUrl;
            UserId = userId;
            FireService = new FirebaseServices(DatabaseUrl);
            LoadDataFromFirebase();
            BtonContent = Lang.Edit;
            UpdateSaveCommand = new AsyncCommand(UpdateSaveCommandActionAsync);
            FireService = new FirebaseServices(DatabaseUrl);
        }


        private async Task UpdateSaveCommandActionAsync() {
            if (BtonContent!.Equals(Lang.Edit)) {
                IsGridViewEnable = true;
                IsReadOnly = false;
                BtonContent = Lang.Update;
            } else {
                await UpdateDataFromFirebase();

                IsGridViewEnable = false;
                IsReadOnly = true;
                BtonContent = Lang.Edit;
            }
        }

        private async void LoadDataFromFirebase() {
            if (!string.IsNullOrEmpty(UserId)) {
                FirebaseUserObject = await FireService.GetAsync(Node, UserId!);
            }
        }

        private async Task UpdateDataFromFirebase() {

            if (FirebaseUserObject != null) {

                var key = FirebaseUserObject.Key;

                FirebaseUserObject.Object.Username = FirebaseUserObject.Object.Username;

                var image = await AzureStorageService.UploadToAzureBlobStorage(
                    Path.GetFullPath(SelectedItem!), FirebaseUserObject.Object.Id.ToLower());

                FirebaseUserObject.Object.PhotoUrl = image;

                await FireService.UpdateAsync(Node, key, FirebaseUserObject);
            }
        }
    }
}
