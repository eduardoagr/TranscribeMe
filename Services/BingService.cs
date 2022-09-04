namespace TranscribeMe.Services;
public class BingService
{
    //static string Key = "73ebd122e47d45bd92d23f7c2c423af7";
    //static string Url = "https://api.bing.microsoft.com/v7.0/spellcheck?proof&en-US";
    //readonly HttpClient Client = new();

    //public BingService()
    //{
    //    Client = new HttpClient();
    //}

    ////public async Task<SpellCheckModel?> BingProcessString(string str)
    ////{
    ////    var text = $"&text={str}";
    ////    var request = new HttpRequestMessage
    ////    {
    ////        Method = HttpMethod.Post,
    ////        RequestUri = new Uri($"{Url}{text}")
    ////    };
    ////    request.Headers.Add("Ocp-Apim-Subscription-Key", Key);


    ////    var res = await Client.GetAsync($"{Url}{text}");
    ////    if (res.IsSuccessStatusCode)
    ////    {
    ////        var model = await res.Content.ReadFromJsonAsync<SpellCheckModel>();
    ////        return model;
    ////    }

    ////    return null;
    ////}
}
