using Brush = System.Windows.Media.Brush;

namespace TranscribeMe.ViewModel {
    public class ReadOutLoudWindowViewModel {
        public string Text { get; set; }

        public Dictionary<Brush, string> ColorPairs { get; set; }

        public ReadOutLoudWindowViewModel(string text) {
            Text = text;
            ColorPairs = ColorHelper.GetColors();
        }


        public ReadOutLoudWindowViewModel() {
            ColorPairs = ColorHelper.GetColors();
        }
    }
}
