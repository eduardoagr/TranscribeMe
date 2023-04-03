namespace TranscribeMe.Services {
    public class SpeechToTextService {

        private string? res;

        public SpeechRecognizer? Recognizer { get; set; }


        public async Task<string> StartRecognitionAsync(string lang) {
            var config = SpeechConfig.FromSubscription(ConstantsHelpers.AZURE_SPEECH_KEY,
                ConstantsHelpers.AZURE_SPEECH_REGION);

            config.SpeechRecognitionLanguage = lang;
            Recognizer = new SpeechRecognizer(config);



            Recognizer.Recognizing += (sender, e) => {
                res = e.Result.Text;
            };

            await Recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);


            return res;
        }

        public void StopRecognition() {
            Recognizer.StopContinuousRecognitionAsync();
        }
    }
}
