namespace TranscribeMe.Helpers {
    public class LanguagesHelper {

        public static Dictionary<int, Languages> GetLanguages() {

            return new Dictionary<int, Languages>() {

                {1, new Languages { Name = "Spanish", Code = "es"} },
                {2, new Languages { Name = "English", Code = "en"} }
            };
        }
    }
}
