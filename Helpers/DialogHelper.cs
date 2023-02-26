
namespace TranscribeMe.Helpers {
    public class DialogHelper {

        public static string GetFilePath(string type) {

            var filter = string.Empty;

            switch (type) {

                case ConstantsHelpers.AUDIOS:
                    filter = ConstantsHelpers.AUDIOFILES;
                    break;
                case ConstantsHelpers.VIDEOS:
                    filter = ConstantsHelpers.VIDEOFILES;
                    break;
                case ConstantsHelpers.IMAGES:
                    filter = ConstantsHelpers.IMAGEFILES;
                    break;
                case ConstantsHelpers.DOCUMENTS:
                    filter = ConstantsHelpers.DOCUMENTSFIILES;
                    break;
            }

            var dialog = new OpenFileDialog {

                Filter = filter,
            };

            var res = dialog.ShowDialog();

            if (res == true) {
                return Path.GetFullPath(dialog.FileName);
            }
            return string.Empty;
        }
    }
}