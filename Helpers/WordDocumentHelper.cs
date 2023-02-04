using Syncfusion.DocIO.DLS;

using System.Text.RegularExpressions;

namespace TranscribeMe.Helpers {

    public class WordDocumentHelper {

        public static string CreateWordDocument(string text, string fileName, bool transcription) {

            var doxcName = $"{fileName}.docx";
            var DocumentFolderPath = string.Empty;
            if (transcription) {
                DocumentFolderPath = FolderHelper.CreateFolder();
            } else {
                DocumentFolderPath = FolderHelper.CreateFolder(ConstantsHelpers.IMAGETEXT);
            }

            var docPath = Path.Combine(DocumentFolderPath, doxcName);

            using WordDocument wordDocument = new();

            wordDocument.EnsureMinimal();

            wordDocument.LastParagraph.AppendText(text);

            // Find all the text which start with capital letters next to period (.) in the Word document.

            //For example . Text or .Text

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