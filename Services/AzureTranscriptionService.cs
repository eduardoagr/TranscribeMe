namespace TranscribeMe.Services {
    public class AzureTranscriptionService {
        public async Task<string?> ConvertToTextAsync(string FilePath, string FileName, string Lang) {

            StringBuilder builder = new();

            var config = SpeechConfig.FromSubscription
                (ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

            //Configure speech recognition

            var taskCompletionSource = new TaskCompletionSource<int>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
            if (!string.IsNullOrEmpty(FileName)) {
                config.SpeechRecognitionLanguage = Lang;

                using var speechRecognizer = new SpeechRecognizer(config, audioConfig);

                speechRecognizer.SessionStopped += (s, e) => {
                    taskCompletionSource.TrySetResult(0);
                };

                speechRecognizer.Recognized += (s, e) => {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                        foreach (var item in e.Result.Text) {
                            builder.Append(item);
                        }
                        // append a space if you want to separate phrases from each other
                    }
                };

                await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
                Task.WaitAny(new[] { taskCompletionSource.Task });
                await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

                return builder.ToString();
            }

            return null;
        }
    }
}
