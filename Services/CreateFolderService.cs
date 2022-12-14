namespace TranscribeMe.Services {
    static class CreateFolderService {

        public static string CreateFolder(string FolderName = ConstantsHelpers.AUDIO) {
            var directoryPath = Directory.CreateDirectory(Path.Combine(
                                   Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                                    ConstantsHelpers.TRANDCRIBED, FolderName));

            return directoryPath.FullName;
        }

    }
}
