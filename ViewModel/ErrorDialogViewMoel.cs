namespace TranscribeMe.ViewModel {
    public class ErrorDialogViewMoel {
        public string Title { get; set; }

        public string Message { get; set; }

        public string CloseBtn { get; set; }

        public ErrorDialogViewMoel(string title, string message, string closeBtn) {
            Title = title;
            Message = message;
            CloseBtn = closeBtn;
        }
    }
}
