namespace TranscribeMe.Services {
    public class AzureTranscriptionService {
        public async Task<string?> ConvertToTextAsync(string FilePath, string FileName, string Lang) {
            if (!string.IsNullOrEmpty(FileName)) {

                StringBuilder builder = new();
                List<char> Characers = new();

                var config = SpeechConfig.FromSubscription
                (ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

                //Configure speech recognition

                var taskCompletionSource = new TaskCompletionSource<int>(
                    TaskCreationOptions.RunContinuationsAsynchronously);

                using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
                config.SpeechRecognitionLanguage = Lang;
                using var speechRecognizer = new SpeechRecognizer(config, audioConfig);


                speechRecognizer.Recognized += (s, e) => {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                        foreach (var item in e.Result.Text) {
                            builder.Append(item);
                        }
                    }
                    speechRecognizer.SessionStarted += (sender, e) => {
                    };
                    speechRecognizer.SessionStopped += (sender, e) => {
                        foreach (var item in Characers) {
                            builder.Append(item);
                        }

                        taskCompletionSource.TrySetResult(0);
                    };
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
