using System.Windows.Interop;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]

    public class FilesCollectionViewModel {
        public Command OpenFileCommand { get; set; }

        public Command TextToSearchCommand { get; set; }

        public AsyncCommand<FileItem> RenameFileCommand { get; set; }

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
            OpenFileCommand = new Command<FileItem>(OpenFileAction);
            ShareCommand = new AsyncCommand<FileItem>(ShareActionAsync);
            RenameFileCommand = new AsyncCommand<FileItem>(RenameFileActionAsync);
            GetFolders();
            RetrieveFiles();
        }

        private void GetFolders() {
            Folders = new List<string> {
                ConstantsHelpers.TRANSLATIONS,
                ConstantsHelpers.TRANSCRIPTIONS,
                ConstantsHelpers.IMAGETEXT,
            };
        }

        private async Task RenameFileActionAsync(FileItem file) {

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

            // Delete the file if the user clicked the primary button.
            /// Otherwise, do nothing.
            if (result == ContentDialogResult.Primary) {
                RenameFile(inputTextBox.Text, fileExt, filePath);
                RetrieveFiles();
            }
        }

        private void RenameFile(string newFileName, string ext, string path) {
            // Get the directory path of the current file
            var directoryPath = Path.GetDirectoryName(path);

            // Create the new file path by combining the directory path, new file name, and file extension
            var newFilePath = Path.Combine(directoryPath!, newFileName + ext);

            // Check if the new file path already exists
            if (!File.Exists(newFilePath)) {
                File.Move(path, newFilePath);
            }
        }
        private async Task ShareActionAsync(FileItem item) {

            var win = Application.Current.MainWindow;
            var interop = DataTransferManager.As<IDataTransferManagerInterop>();
            var hwnd = new WindowInteropHelper(win).Handle;

            var pManager = interop.GetForWindow(hwnd, new("A5CAEE9B-8708-49D1-8D36-67D25A8DA00C"));
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

                RetrieveFiles();

                var FilteredItems = FilesCollection.Where(item =>
                item.FileName.ToLowerInvariant().Contains(SeachTerm.ToLowerInvariant())).ToList();

                FilesCollection.Clear();

                foreach (var Item in FilteredItems) {
                    FilesCollection.Add(Item);
                }
            } else {
                RetrieveFiles();
            }
        }

        private void OpenFileAction(FileItem file) {
            if (file != null) {
                Process p = new();
                p.StartInfo.FileName = file.FilePath;
                p.StartInfo.UseShellExecute = true;
                p.Start();
            } else {
                // Show a message box or log the error
                MessageBox.Show("The file does not exist or cannot be accessed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RetrieveFiles() {

            string TranscribedDocsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
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
                                System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
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