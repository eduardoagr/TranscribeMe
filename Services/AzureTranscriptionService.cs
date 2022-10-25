namespace TranscribeMe.Services {
    public class AzureTranscriptionService {

        public async Task<string> ConvertToTextAsync(string FilePath, string FileName, string Lang) {

            List<char> Characers = new();

            StringBuilder builder = new();

            var config = SpeechConfig.FromSubscription
                (ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

            //Configure speech recognition

            var taskCompleteionSource = new TaskCompletionSource<int>();

            using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
            if (!string.IsNullOrEmpty(FileName)) {
                using var speechRecognizer = new SpeechRecognizer(config, audioConfig);

                config.EnableDictation();
                config.SpeechRecognitionLanguage = Lang;


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
                };


                await speechRecognizer.StartContinuousRecognitionAsync()
                    .ConfigureAwait(false);

                Task.WaitAny(new[] { taskCompleteionSource.Task });

                await speechRecognizer.StopContinuousRecognitionAsync()
                    .ConfigureAwait(false);

                return builder.ToString();
            }
        }
    }
}