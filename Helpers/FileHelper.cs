namespace TranscribeMe.Helpers {
    public class FileHelper {

        public static void CloseFileUsed(FileItem file) {
            try {
                using FileStream stream = File.Open(file.FilePath, FileMode.OpenOrCreate,
                    FileAccess.Read, FileShare.None);
                stream.Close();
            } catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32) {
                using FileStream stream = File.Open(file.FilePath,
                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                stream.Close();
            }
        }

        public static bool IsFileLocked(FileItem fileItem) {

            try {
                using FileStream stream = File.Open(fileItem.FilePath, FileMode.OpenOrCreate,
                    FileAccess.Read, FileShare.None);
                stream.Close();
            } catch (IOException) {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            return false;
        }
    }
}
