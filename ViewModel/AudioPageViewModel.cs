namespace TranscribeMe.ViewModel {
    public class AudioPageViewModel {

        public List<string>? Languages { get; set; }

        public AudioPageViewModel() {
            InitListLanguages();
        }

        private void InitListLanguages() {

            Languages = new List<string>() {
                "English",
                "Spanish"
            };
        }
    }
}
