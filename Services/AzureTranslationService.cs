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


            MessageBox.Show($"  Status: {operation.Status}");
            MessageBox.Show($"  Created on: {operation.CreatedOn}");
            MessageBox.Show($"  Last modified: {operation.LastModified}");
            MessageBox.Show($"  Total documents: {operation.DocumentsTotal}");
            MessageBox.Show($"    Succeeded: {operation.DocumentsSucceeded}");
            MessageBox.Show($"    Failed: {operation.DocumentsFailed}");
            MessageBox.Show($"    In Progress: {operation.DocumentsInProgress}");
            MessageBox.Show($"Not started: {operation.DocumentsNotStarted}");

        }
    }
}
