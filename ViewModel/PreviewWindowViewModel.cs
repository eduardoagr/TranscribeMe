using Brush = System.Windows.Media.Brush;

namespace TranscribeMe.ViewModel {
    public class PreviewWindowViewModel {
        public string Text { get; set; }

        public Dictionary<Brush, string> ColorPairs { get; set; }

        public PreviewWindowViewModel(string text) {
            Text = text;
            ColorPairs = ColorHelper.GetColors();
        }


        public PreviewWindowViewModel() {
            ColorPairs = ColorHelper.GetColors();
        }
    }
}
