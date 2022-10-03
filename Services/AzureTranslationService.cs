using Azure;
using Azure.AI.Translation.Document;

using Config;

namespace TranscribeMe.Services {
    public class AzureTranslationService {

        public static async Task TranslatorAsync(Uri sourceUrl, Uri TargetUrl, string language = "es") {

            DocumentTranslationClient client = new(new Uri(ConstantsHelpers.ENDPOINT), new AzureKeyCredential(ConstantsHelpers.KEY));

            var input = new DocumentTranslationInput(sourceUrl, TargetUrl, language);

            DocumentTranslationOperation operation = await client.StartTranslationAsync(input);

            await operation.WaitForCompletionAsync();

            await foreach (var document in operation.Value) {
                MessageBox.Show($"Document with Id: {document.Id}");
                MessageBox.Show($"  Status:{document.Status}");
                if (document.Status == DocumentTranslationStatus.Succeeded) {
                    MessageBox.Show($"  Translated Document Uri: {document.TranslatedDocumentUri}");
                    MessageBox.Show($"  Translated to language: {document.TranslatedToLanguageCode}.");
                    MessageBox.Show($"  Document source Uri: {document.SourceDocumentUri}");
                } else {
                    MessageBox.Show($"  Error Code: {document.Error.Code}");
                    MessageBox.Show($"  Message: {document.Error.Message}");
                }
            }

        }
    }
}
