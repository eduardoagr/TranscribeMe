using Azure;
using Azure.AI.Translation.Document;

namespace TranscribeMe.Services {
    public class AzureTranslationService {

        public static async Task<bool> TranslatorAsync(Uri sourceUrl, Uri TargetUrl, int id, ObservableCollection<Tile> tiles, string language = "es") {

            tiles[id].IsTileActive = false;

            DocumentTranslationClient client = new(new Uri(ConstantsHelpers.ENDPOINT), new AzureKeyCredential(ConstantsHelpers.KEY));

            var input = new DocumentTranslationInput(sourceUrl, TargetUrl, language);

            DocumentTranslationOperation operation = await client.StartTranslationAsync(input);

            await operation.WaitForCompletionAsync();

            tiles[id].IsTileActive = true;

            if (operation.Status == DocumentTranslationStatus.Succeeded) {
                return true;
            }
            return false;
        }
    }
}
