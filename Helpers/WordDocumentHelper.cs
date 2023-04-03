using System.Text.RegularExpressions;

namespace TranscribeMe.Helpers {

    public class WordDocumentHelper {

        public static string CreateWordDocument(string text, string fileName, string folderName, bool transcription) {

            var doxcName = $"{fileName}.docx";
            var DocumentFolderPath = string.Empty;

            DocumentFolderPath = FolderHelper.CreateFolder(folderName);

            var docPath = Path.Combine(DocumentFolderPath, doxcName);

            using WordDocument wordDocument = new();

            wordDocument.EnsureMinimal();

            wordDocument.LastParagraph.AppendText(text);

            if (transcription) {

                var textSelections = wordDocument.FindAll(new Regex(@"[.]\s+[A-Z]|[.][A-Z]"));

                for (int i = 0; i < textSelections.Length; i++) {

                    WTextRange textToFind = textSelections[i].GetAsOneRange();

                    string replacementText = textToFind.Text;

                    textToFind.Text = replacementText;
                }
            }
            wordDocument.Save(docPath);

            return docPath;
        }
    }
}