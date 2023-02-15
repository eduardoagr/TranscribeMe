

using Brush = System.Windows.Media.Brush;

namespace TranscribeMe.ViewModel.Windows {

    [AddINotifyPropertyChangedInterface]
    public class PreviewWindowViewModel {

        private readonly AzureTextToSpeechService azureTextToSpeechService;

        public AsyncCommand SpeakButtonCommand { get; set; }

        public AsyncCommand StopSpeakButtonCommand { get; set; }

        public bool IsStopEnabled { get; set; }

        public bool IsPlayingEnabled { get; set; }

        public string Text { get; set; }

        public Dictionary<Brush, string> ColorPairs { get; set; }

        public PreviewWindowViewModel(string text) {
            azureTextToSpeechService = new AzureTextToSpeechService();
            IsPlayingEnabled = true;
            IsStopEnabled = false;
            Text = text;
            ColorPairs = ColorHelper.GetColors();
            SpeakButtonCommand = new AsyncCommand(SpeakButtonctionAsync);
            StopSpeakButtonCommand = new AsyncCommand(StopSpeakButtonActionAsync);
        }

        private async Task StopSpeakButtonActionAsync() {
            azureTextToSpeechService.StopSpeaking();
            IsPlayingEnabled = true;
            IsStopEnabled = false;
        }

        private async Task SpeakButtonctionAsync() {
            await azureTextToSpeechService.ReadOutLoudAsync(Text);
            IsPlayingEnabled = false;
            IsStopEnabled = true;
        }

        public PreviewWindowViewModel() {
            ColorPairs = ColorHelper.GetColors();
        }
    }
}
