namespace TranscribeMe.Helpers {
    public class LanguagesHelper {

        public static Dictionary<int, Languages> GetLanguages() {

            return new Dictionary<int, Languages>() {

                {1, new Languages { Name = "Spanish", Code = "es-ES"} },
                {2, new Languages { Name = "English", Code = "en-US"} }
            };
        }
    }
}
