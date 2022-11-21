namespace TranscribeMe.Services {
    public class AzureTranscriptionService {
        public async Task<string?> ConvertToTextAsync(string FilePath, string FileName, string Lang) {
            List<char> Characers = new();

            StringBuilder builder = new();

            var config = SpeechConfig.FromSubscription
                (ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

            //Configure speech recognition

            var speechRecognizerWaiter = new TaskCompletionSource<string>();

            using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
            if (!string.IsNullOrEmpty(FileName)) {
                config.EnableDictation();
                config.SpeechRecognitionLanguage = Lang;
                config.OutputFormat = OutputFormat.Detailed;

                using var speechRecognizer = new SpeechRecognizer(config, audioConfig);

                speechRecognizer.Recognized += (sender, e) => {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                        foreach (var item in e.Result.Text) {
                            Characers.Add(item);
                        }
                    }
                };

                speechRecognizer.SessionStarted += (sender, e) => {

                    Debug.WriteLine("-----------> started");
                };

                speechRecognizer.SessionStopped += (sender, e) => {

                    Debug.WriteLine("-----------> stooped");

                    foreach (var item in Characers) {
                        builder.Append(item);
                    }

                    Debug.WriteLine(builder.ToString());

                    speechRecognizerWaiter.TrySetResult(builder.ToString());
                };

                await speechRecognizer.StartContinuousRecognitionAsync();

                var str = await speechRecognizerWaiter.Task;

                await speechRecognizer.StopContinuousRecognitionAsync();

                return str;
            }

            return null;
        }
    }
}
