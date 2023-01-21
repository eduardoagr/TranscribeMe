
namespace TranscribeMe.Model {
    public class FileItem {

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileLenght { get; set; }

        public ImageSource FileIcon { get; set; }

        public FileItem(string fileName, string fileLenght, ImageSource fileIcon, string filePath) {
            FileName = fileName;
            FileLenght = fileLenght;
            FileIcon = fileIcon;
            FilePath = filePath;
        }
    }
}
