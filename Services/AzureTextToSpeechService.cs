﻿namespace TranscribeMe.Services {

    public class AzureTextToSpeechService {

        SpeechSynthesizer speechSynthesizer;

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


            string logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logFile.log");
            config.SetProperty(PropertyId.Speech_LogFilename, logFile);

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



            speechSynthesizer = new SpeechSynthesizer(config);
            await speechSynthesizer.StartSpeakingTextAsync(text);

        }


        public async Task StopSpeechAsync() {
            await speechSynthesizer.StopSpeakingAsync();
        }

    }
}
