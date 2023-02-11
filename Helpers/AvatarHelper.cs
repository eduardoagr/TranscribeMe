namespace TranscribeMe.Helpers {
    public class AvatarHelper {
        public static ObservableCollection<ImageSource> GetAvatars() {
            var avatars = new ObservableCollection<ImageSource>();
            for (int i = 0; i < 25; i++) {
                avatars.Add(new BitmapImage(
                    new Uri($"/Images/{i}.png", UriKind.Relative)));
            }
            return avatars;
        }
    }
}
