namespace TranscribeMe.Model {
    public class FirebaseResponse {
        public Error error { get; set; }
    }

    public class Error {
        public int code { get; set; }
        public string message { get; set; }
        public List<ErrorDetail> errors { get; set; }
    }

    public class ErrorDetail {
        public string message { get; set; }
        public string domain { get; set; }
        public string reason { get; set; }
    }
}
