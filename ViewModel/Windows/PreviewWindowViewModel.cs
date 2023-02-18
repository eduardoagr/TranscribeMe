using Brush = System.Windows.Media.Brush;

namespace TranscribeMe.ViewModel.Windows {

    [AddINotifyPropertyChangedInterface]
    public class PreviewWindowViewModel {

        private AzureTextToSpeechService AzureTextToSpeech;

        public AsyncCommand SpeakButtonCommand { get; set; }

        public AsyncCommand StopSpeakButtonCommand { get; set; }

        public bool IsStopEnabled { get; set; }

        public bool IsPlayingEnabled { get; set; }

        public string Text { get; set; }

        public Dictionary<Brush, string> ColorPairs { get; set; }

        public PreviewWindowViewModel(string str) {
            IsPlayingEnabled = true;
            AzureTextToSpeech = new AzureTextToSpeechService();
            IsStopEnabled = false;
            Text = str;
            ColorPairs = ColorHelper.GetColors();
            SpeakButtonCommand = new AsyncCommand(SpeakButtonction);
            StopSpeakButtonCommand = new AsyncCommand(StopSpeakButtonAction);
        }

        private async Task StopSpeakButtonAction() {
            await AzureTextToSpeech.StopSpeechAsync();
            IsPlayingEnabled = true;
            IsStopEnabled = false;
        }

        private async Task SpeakButtonction() {
            await AzureTextToSpeech.ReadOutLoudAsync(Text);
            IsPlayingEnabled = false;
            IsStopEnabled = true;
        }

        public PreviewWindowViewModel() {
            ColorPairs = ColorHelper.GetColors();
        }
    }
}
