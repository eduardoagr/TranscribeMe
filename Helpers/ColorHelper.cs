using Brush = System.Windows.Media.Brush;

namespace TranscribeMe.Helpers {
    public class ColorHelper {

        public static Dictionary<Brush, string> GetColors() {

            return new Dictionary<Brush, string>() {

                { new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 0)), Lang.Yellow },
                { new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0)), Lang.Red },
                { new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 0)), Lang.Green },
                { new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 255)), Lang.Blue }
            };
        }
    }
}