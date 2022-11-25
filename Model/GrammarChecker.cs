namespace TranscribeMe.Model {

    public class Suggestion {
        public string suggestion { get; set; }
        public double score { get; set; }
    }

    public class FlaggedToken {
        public int offset { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public IList<Suggestion> suggestions { get; set; }
    }

    public class SpellCheckModel {
        public string _type { get; set; }
        public IList<FlaggedToken> flaggedTokens { get; set; }
    }

}
