namespace TranscribeMe.Services {
    public class AzureTranscriptionService {

        public async Task ConvertToTextAsync(string FilePath, string FileName, int Id, ObservableCollection<Tile> tiles, List<char> Characers) {
            //Configure speech service

            var config = SpeechConfig.FromSubscription(ConstantsHelpers.AZURE_KEY, ConstantsHelpers.AZURE_REGION);

            config.EnableDictation();

            //Configure speech recognition

            var taskCompleteionSource = new TaskCompletionSource<int>();

            using var audioConfig = AudioConfig.FromWavFileInput(FilePath);
            if (!string.IsNullOrEmpty(FileName)) {
                using var speechRecognizer = new SpeechRecognizer(config, audioConfig);

                speechRecognizer.Recognized += (sender, e) => {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech) {
                        foreach (var item in e.Result.Text) {
                            Characers.Add(item);
                        }
                    }
                };

                speechRecognizer.SessionStarted += (sender, e) => {

                    tiles![Id].IsTileActive = false;
                };

                speechRecognizer.SessionStopped += (sender, e) => {

                    const string ext = ".docx";

                    var pathToSave = CreateFolderService.CreateFolder(ConstantsHelpers.TRANSCRIPTIONS);

                    var filename = Path.Combine(pathToSave, $"{Path.GetFileNameWithoutExtension(FilePath)}{ext}");

                    var sb = new StringBuilder();

                    foreach (var item in Characers) {
                        sb.Append(item);
                    }

                    using var document = new WordDocument();

                    document.EnsureMinimal();

                    document.LastParagraph.AppendText(sb.ToString());

                    // Find all the text which start with capital letters next to period (.) in the Word document.

                    //For example . Text or .Text

                    TextSelection[] textSelections = document.FindAll(new Regex(@"[.]\s+[A-Z]|[.][A-Z]"));

                    for (int i = 0; i < textSelections.Length; i++) {

                        WTextRange textToFind = textSelections[i].GetAsOneRange();

                        //Replace the period (.) with enter(\n).

                        string replacementText = textToFind.Text.Replace(".", ".\n\n");

                        textToFind.Text = replacementText;

                    }

                    document.Save(filename);

                    ToastService.CreateAndShowPrompt();

                    tiles![Id].IsTileActive = true;


                };

                await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                Task.WaitAny(new[] { taskCompleteionSource.Task });

                await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }
    }
}
