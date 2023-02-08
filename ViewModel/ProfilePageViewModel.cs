namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class ProfilePageViewModel {

        public ObservableCollection<FirebaseObject<LocalUser>>? UserData { get; set; }

        public bool IsReadOnly { get; set; }

        public string? BtonContent { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }

        public ProfilePageViewModel(string userId) {

            UserId = userId;

            LoadDataFromFirebase();
            BtonContent = Lang.Edit;
            var age = GetAge(DateOfBirth);
        }

        public int GetAge(DateTime dateOfBirth) {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }


        private async void LoadDataFromFirebase() {

            UserData = new ObservableCollection<FirebaseObject<LocalUser>>();

            var firebase = new FirebaseClient(
                "https://transcribed-c69c8-default-rtdb.europe-west1.firebasedatabase.app/");

            var FireUser = await firebase
               .Child("Users")
               .OnceAsync<LocalUser>();


            UserData.Clear();
            foreach (var item in FireUser) {
                if (item.Object.Id == UserId) {
                    UserData.Add(item);
                }

            }

            if (UserData.Count > 0) {
                var user = UserData.First().Object;
                DateOfBirth = user.DateOfBirth;
            }
        }
    }
}
