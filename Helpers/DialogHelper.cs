namespace TranscribeMe.Helpers {
    public class DialogHelper {

        public string GetFilePath(string type) {

            var filter = string.Empty;

            switch (type) {

                case ConstantsHelpers.AUDIO:
                    filter = ConstantsHelpers.AUDIOFILES;
                    break;
                case ConstantsHelpers.VIDEO:
                    filter = ConstantsHelpers.VIDEOFILES;
                    break;
                case ConstantsHelpers.IMAGES:
                    filter = ConstantsHelpers.IMAGES;
                    break;
                case ConstantsHelpers.DOCUMENTS:
                    filter = ConstantsHelpers.DOCUMENTS;
                    break;
            }

            var dialog = new OpenFileDialog()
            {

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