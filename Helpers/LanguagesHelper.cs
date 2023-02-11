namespace TranscribeMe.Helpers {
    public class LanguagesHelper {

        public static Dictionary<string, string> GetLanguages() {

            return new Dictionary<string, string>() {

                { Lang.ES, "es-ES"} ,
                { Lang.EN , "en-US" }
            };
        }
    }
}
