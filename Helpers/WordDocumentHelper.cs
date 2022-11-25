using Syncfusion.DocIO.DLS;

namespace TranscribeMe.Helpers {

    public class WordDocumentHelper {

        public string CreateWordDocument(string text, string fileName, FolderHelper folderHelper) {

            var doxcName = $"{fileName}.docx";
            var DocumentFolderPath = folderHelper.CreateFolder();

            var docPath = Path.Combine(DocumentFolderPath, doxcName);

            using var wordDocument = new WordDocument();

            wordDocument.EnsureMinimal();

            wordDocument.LastParagraph.AppendText(text);

            wordDocument.Save(docPath);

            return docPath;
        }
    }
}
