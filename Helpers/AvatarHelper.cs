namespace TranscribeMe.Helpers {
    public class AvatarHelper {
        public static ObservableCollection<string> GetAvatars() {
            var avatars = new ObservableCollection<string>();
            for (int i = 0; i < 26; i++) {
                var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", $"{i}.png");
                avatars.Add(imagePath);
            }
            return avatars;
        }
    }
}
