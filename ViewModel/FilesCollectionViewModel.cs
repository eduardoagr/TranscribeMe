using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

using System.Windows.Interop;

using TranscribeMe.Services;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

using static System.Net.Mime.MediaTypeNames;

using Application = System.Windows.Application;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]

    public class FilesCollectionViewModel {
        public Command OpenFileCommand { get; set; }

        public Command TextToSearchCommand { get; set; }

        public Command DeleteCommand { get; set; }

        public AsyncCommand<FileItem> ReadCommmand { get; set; }

        public AsyncCommand<FileItem> RenameCommand { get; set; }

        public AsyncCommand<FileItem> ShareCommand { get; set; }

        public ObservableCollection<FileItem> FilesCollection { get; set; }

        public ObservableCollection<string> FilteredItems { get; set; }

        public FileItem? SelectedFile { get; set; }

        public List<string>? Folders { get; set; }

        public string[] FilesLength { get; set; } = { "B", "KB", "MB", "GB", "TB" };


        public bool IsContextMenuOpen;

        public FilesCollectionViewModel() {
            FilesCollection = new ObservableCollection<FileItem>();
            FilteredItems = new ObservableCollection<string>();
            TextToSearchCommand = new Command<string>(SearchAction);
            OpenFileCommand = new Command<FileItem>(OpenAction);
            ReadCommmand = new AsyncCommand<FileItem>(ReadOutLoudActionAsync);
            DeleteCommand = new Command<FileItem>(DeleteAction);
            ShareCommand = new AsyncCommand<FileItem>(ShareActionAsync);
            RenameCommand = new AsyncCommand<FileItem>(RenameActionAsync);
            GetFolders();
            GetFiles();
        }

        private void GetFolders() {
            Folders = new List<string> {
                ConstantsHelpers.TRANSLATIONS,
                ConstantsHelpers.TRANSCRIPTIONS,
                ConstantsHelpers.IMAGETEXT,
                ConstantsHelpers.AUDIOS
            };
        }

        private async Task RenameActionAsync(FileItem file) {

            var name = Path.GetFileNameWithoutExtension(file.FilePath);
            var fileExt = Path.GetExtension(file.FilePath);
            var filePath = file.FilePath;

            var inputTextBox = new TextBox {
                AcceptsReturn = false,
                Text = name,
                VerticalAlignment = VerticalAlignment.Center
            };
            var dialog = new ContentDialog {
                Content = inputTextBox,
                Title = "Rename File",
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Rename",
                SecondaryButtonText = "Cancel"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) {
                RenameFile(inputTextBox.Text, fileExt, filePath);
                GetFiles();
            }
        }

        private static void RenameFile(string newFileName, string ext, string path) {

            // Get the directory path of the current file
            var directoryPath = Path.GetDirectoryName(path);

            // Create the new file path by combining the directory path, new file name, and file extension
            var newFilePath = Path.Combine(directoryPath!, newFileName + ext);

            // Check if the new file path already exists
            if (!File.Exists(newFilePath)) {
                File.Move(path, newFilePath);
            }
        }

        private void DeleteAction(FileItem file) {

            // Check if the new file path already exists
            if (File.Exists(file.FilePath)) {
                File.Delete(file.FilePath);
            }
            GetFiles();
        }

        private async Task ReadOutLoudActionAsync(FileItem file) {

            var win = new Window {
                Width = 800
            };

            RichTextBox richTextBox = new() {
                Margin = new Thickness(20)
            };

            await GetTextAsync(file, richTextBox);

            win.Content = richTextBox;
            win.Show();

            if (win.IsVisible) {
                //call readservice
            }

        }

        private static async Task GetTextAsync(FileItem file, RichTextBox richTextBox) {

            StringBuilder sb = new();

            switch (Path.GetExtension(file.FilePath)) {

                case ".pdf":
                    PdfLoadedDocument pdfDoc = new(File.ReadAllBytes(file.FilePath));
                    foreach (PdfLoadedPage loadedPage in pdfDoc.Pages) {
                        var text = loadedPage.ExtractText();
                        text = text.Trim();
                        await BingSpellCheckService.SpellingCorrector(text);
                        sb.Append(text);
                    }
                    richTextBox.AppendText(sb.ToString());
                    break;
                case ".docx":
                case ".doc":
                    WordDocument wordDocument = new WordDocument(file.FilePath);
                    foreach (IWSection section in wordDocument.Sections) {
                        foreach (IWParagraph paragraph in section.Body.Paragraphs) {
                            string text = paragraph.Text;
                            await BingSpellCheckService.SpellingCorrector(text);
                            sb.Append(text);
                        }
                    }
                    richTextBox.AppendText(sb.ToString());
                    break;
            }
        }

        private async Task ShareActionAsync(FileItem item) {

            var win = Application.Current.Windows[0];
            var interop = DataTransferManager.As<IDataTransferManagerInterop>();
            var hwnd = new WindowInteropHelper(win).Handle;

            var pManager = interop.GetForWindow(hwnd, new(
                "A5CAEE9B-8708-49D1-8D36-67D25A8DA00C"));

            var manager = DataTransferManager.FromAbi(pManager);
            Marshal.Release(pManager);
            interop.ShowShareUIForWindow(hwnd);

            var file = await StorageFile.GetFileFromPathAsync(item.FilePath);

            manager.DataRequested += (sender, args) => {
                var request = args.Request;
                var data = request.Data;

                data.Properties.Title = "Share Text Example";
                data.Properties.Description = "An example of how to share text.";

                data.SetStorageItems(new[] { file });
            };
        }

        private void SearchAction(string SeachTerm) {

            if (!string.IsNullOrWhiteSpace(SeachTerm)) {

                GetFiles();

                var FilteredItems = FilesCollection.Where(item =>
                item.FileName.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FilesCollection.Clear();

                foreach (var Item in FilteredItems) {
                    FilesCollection.Add(Item);
                }
            } else {
                GetFiles();
            }
        }

        private void OpenAction(FileItem file) {
            if (file != null) {
                Process p = new();
                p.StartInfo.FileName = file.FilePath;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            }
        }

        private void GetFiles() {

            string TranscribedDocsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                ConstantsHelpers.TRANDCRIBED);

            if (Directory.Exists(TranscribedDocsPath)) {

                FilesCollection.Clear();

                foreach (var folder in Folders!) {

                    var rootFolderPath = Path.Combine(TranscribedDocsPath, folder);

                    if (Directory.Exists(rootFolderPath)) {

                        string[] files = Directory.GetFiles(rootFolderPath);

                        foreach (string file in files) {
                            FilesCollection.Add(new FileItem(
                                Path.GetFileName(file),
                                GetFileSize(file),
                                Imaging.CreateBitmapSourceFromHIcon(
                                Icon.ExtractAssociatedIcon(file)!.Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions()),
                                Path.GetFullPath(file)));
                        }
                    }
                }
            }
        }

        private string GetFileSize(string filename) {
            double len = new FileInfo(filename).Length;
            int order = 0;
            while (len >= 1024 && order < FilesLength.Length - 1) {
                order++;
                len /= 1024;
            }
            string result = string.Format("{0:0.##} {1}", len, FilesLength[order]);

            return result;
        }
    }
}