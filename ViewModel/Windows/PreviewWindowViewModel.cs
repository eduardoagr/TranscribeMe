namespace TranscribeMe.ViewModel.Windows {

    [AddINotifyPropertyChangedInterface]
    public class PreviewWindowViewModel {

        private StringBuilder? sb;

        public Visibility isDocument { get; set; } = Visibility.Collapsed;

        public Visibility isPDF { get; set; } = Visibility.Collapsed;

        public Stream? PdfData { get; set; }

        public string? Text { get; set; }

        public PreviewWindowViewModel(FileItem file) {

            sb = new();

            Text = GetFile(file);
        }

        public PreviewWindowViewModel() { }

        private string GetFile(FileItem file) {
            string fileExtension = Path.GetExtension(file.FilePath);

            switch (fileExtension) {
                case ".doc":
                case ".docx":
                    if (FileHelper.IsFileLocked(file)) {
                        FileHelper.CloseFileUsed(file);

                        WordDocument wordDocument = new(file.FilePath);
                        sb.Append(wordDocument.GetText());
                        Text = sb.ToString();
                        isDocument = Visibility.Visible;
                        return Text;

                    } else {

                        WordDocument wordDocument = new(file.FilePath);
                        sb.Append(wordDocument.GetText());
                        Text = sb.ToString();
                        isDocument = Visibility.Visible;
                        return Text;
                    }

                case ".pdf":
                    if (FileHelper.IsFileLocked(file)) {
                        try {
                            FileHelper.CloseFileUsed(file);
                            PdfData = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            isPDF = Visibility.Visible;
                        } catch (IOException ex) {
                            // Log the error or show a message to the user
                            Debug.WriteLine(ex.Message);
                        }
                    } else {

                        PdfData = new FileStream(file.FilePath, FileMode.OpenOrCreate);
                        isPDF = Visibility.Visible;
                    }

                    break;
            }

            return string.Empty;
        }
    }
}
