namespace TranscribeMe.Services {

    public class AzureTextToSpeechService {

        private SpeechSynthesizer? synthesizer;
        private bool IsSpeaking;

        public async Task ReadOutLoudAsync(string text) {

            var key = ConstantsHelpers.AZURE_LANGUAGE_KEY;
            var endpoint = new Uri(ConstantsHelpers.AZURE_LANGUAGE_URL);

            AzureKeyCredential credentials = new(key);
            TextAnalyticsClient textClient = new(endpoint, credentials);

            Response<DetectedLanguage> response = textClient.DetectLanguage(text);

            DetectedLanguage lang = response.Value;

            var config = SpeechConfig.FromSubscription(
            ConstantsHelpers.AZURE_SPEECH_KEY,
            ConstantsHelpers.AZURE_SPEECH_REGION);

            switch (lang.Iso6391Name) {
                case "en":
                    config.SpeechSynthesisVoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, SaraNeural)";
                    config.SpeechSynthesisLanguage = "en-us";
                    break;
                case "es":
                    config.SpeechSynthesisVoiceName = "Microsoft Server Speech Text to Speech Voice (es-VE, PaolaNeural)";
                    config.SpeechSynthesisLanguage = "es-VE";
                    break;
                default:
                    break;
            }

            synthesizer = new SpeechSynthesizer(config);
            IsSpeaking = true;
            var result = await synthesizer.StartSpeakingTextAsync(text);

            if (!IsSpeaking) {
                await synthesizer.StopSpeakingAsync();
            }
        }




        public void StopSpeaking() {

            IsSpeaking = false;
        }
    }
}
