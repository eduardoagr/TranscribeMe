namespace TranscribeMe.Services {
    public class BingSpellCheckService {

        public static async Task<string> SpellingCorrector(string txt, [Optional] string lang) {

            var mode = "proof";

            var text = txt.Trim();
            var url = $"{ConstantsHelpers.BING_SPELL_URL}?mode={mode}&mkt={lang}&text={txt}";

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();

            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(url);
            request.Headers.Add("Ocp-Apim-Subscription-Key", ConstantsHelpers.BING_SPELL_KEY);

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) {

                var data = await response.Content.ReadFromJsonAsync<SpellCheckModel>();

                foreach (var item in data!.flaggedTokens) {
                    text = text.Replace(item.token, item.suggestions[0].suggestion);
                }
            }
            return text;
        }
    }
}
