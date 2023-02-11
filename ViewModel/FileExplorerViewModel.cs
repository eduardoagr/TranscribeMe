using TranscribeMe.View.Dialogs;

using Application = System.Windows.Application;
using ListView = System.Windows.Controls.ListView;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]

    public class FileExplorerViewModel {


        #region Commands

        public Command OpenFileCommand { get; set; }

        public Command TextToSearchCommand { get; set; }

        public Command DeleteCommand { get; set; }

        public Command<FileItem> PreviewCommand { get; set; }

        public Command<ListView> ItemChangedCommand { get; set; }

        public AsyncCommand<FileItem> RenameCommand { get; set; }

        public AsyncCommand<FileItem> ShareCommand { get; set; }

        #endregion

        public ObservableCollection<FileItem> FilesCollection { get; set; }

        public ObservableCollection<string> FilteredItems { get; set; }

        public Visibility IsMenuOpen { get; set; }

        public Visibility IsReadVisible { get; set; }

        public FileItem? SelectedFile { get; set; }

        public List<string>? Folders { get; set; }

        public string[] FilesLength { get; set; } = { "B", "KB", "MB", "GB", "TB" };

        public string? PreviewFileText { get; set; }

        public FileExplorerViewModel() {

            PreviewFileText = Lang.Preview;
            IsMenuOpen = Visibility.Collapsed;
            FilesCollection = new ObservableCollection<FileItem>();
            FilteredItems = new ObservableCollection<string>();
            TextToSearchCommand = new Command<string>(SearchAction);
            OpenFileCommand = new Command<FileItem>(OpenAction);
            ItemChangedCommand = new Command<ListView>(ItemChangedAction);
            PreviewCommand = new Command<FileItem>(PreviewAction);
            DeleteCommand = new Command<FileItem>(DeleteAction);
            ShareCommand = new AsyncCommand<FileItem>(ShareActionAsync);
            RenameCommand = new AsyncCommand<FileItem>(RenameActionAsync);
            GetFolders();
            GetFiles();

        }

        private void ItemChangedAction(ListView obj) {

            var item = obj!.SelectedItem as FileItem;

            if (item == null) {

                return;
            }

            IsMenuOpen = Visibility.Visible;

            if (Path.GetExtension(item!.FilePath) == ".wav") {
                IsReadVisible = Visibility.Collapsed;
            } else {
                IsReadVisible = Visibility.Visible;
            }
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
            if (file == null) { return; }
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
            if (file == null || IsFileLocked(file) == true) { return; }
            // Check if the new file path already exists
            if (File.Exists(file!.FilePath)) {
                File.Delete(file.FilePath);
            }
            GetFiles();
        }

        private static bool IsFileLocked(FileItem file) {
            try {
                using FileStream stream = File.Open(file.FilePath, FileMode.OpenOrCreate,
                    FileAccess.Read, FileShare.None);
                stream.Close();
            } catch (IOException) {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        private async void PreviewAction(FileItem file) {
            if (file == null) { return; }

            if (Path.GetExtension(file.FilePath) == ".mp4") {

                var prev = new PreviewDialog {
                    DataContext = new PreviewDialogViewModel(new Uri(file.FilePath)),
                    PrimaryButtonText = "Close"
                };

                prev.MediaPlayer.Play();

                await prev.ShowAsync();

            } else {
                var str = GetText(file.FilePath);
                if (Path.GetExtension(file.FilePath) != ".wav") {
                    var prevWindow = new PreviewWindow {
                        DataContext = new PreviewWindowViewModel(str)
                    };
                    prevWindow.Show();
                }
            }
        }

        private static string GetText(string filepath) {

            StringBuilder sb = new();

            switch (Path.GetExtension(filepath)) {

                case ".pdf":
                    PdfLoadedDocument pdfDoc = new(File.ReadAllBytes(filepath));
                    foreach (PdfLoadedPage loadedPage in pdfDoc.Pages) {
                        var text = loadedPage.ExtractText();
                        text = text.Trim();
                        sb.Append(text);
                    }
                    break;
                case ".docx":
                case ".doc":
                    WordDocument wordDocument = new(filepath);
                    foreach (IWSection section in wordDocument.Sections) {
                        foreach (IWParagraph paragraph in section.Body.Paragraphs) {
                            string text = paragraph.Text;
                            sb.Append(text);
                        }
                    }
                    break;
            }

            return sb.ToString();
        }

        private async Task ShareActionAsync(FileItem item) {
            if (item == null) { return; }

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