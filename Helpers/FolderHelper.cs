namespace TranscribeMe.Helpers {
    public class FolderHelper {

        public string CreateFolder(string FolderName = ConstantsHelpers.TRANSCRIPTIONS) {
            var directoryPath = Directory.CreateDirectory(Path.Combine(
                                   Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                                    ConstantsHelpers.TRANDCRIBED, FolderName));

            return directoryPath.FullName;
        }

    }
}
