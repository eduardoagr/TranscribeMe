using System.Net.Http;
using System.Net.Http.Json;

namespace TranscribeMe.Helpers {
    public class WordDocumentHelper {
        public async Task CreateWordDocument(string text, string lang, /*string fileName*/ string extention = "docx") {

            var mode = "proof";
            string? cc;
            if (lang.Equals("en")) {
                cc = "US";
            } else {
                cc = "ES";
            }

            var txt = text.Trim();
            var url = $"{ConstantsHelpers.BING_SPELL_URL}?mode={mode}&text={txt}&cc={cc}";

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();

            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", ConstantsHelpers.BING_SPELL_KEY);

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) {

                var data = await response.Content.ReadFromJsonAsync<SpellCheckModel>();

                foreach (var item in data!.flaggedTokens) {
                    txt = txt.Replace(item.token, item.suggestions[0].suggestion);
                }
            }
        }
    }
}
