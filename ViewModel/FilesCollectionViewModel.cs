namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]

    public class FilesCollectionViewModel {
        public Command OpenFileCommand { get; set; }

        public Command TextToSearchCommand { get; set; }

        public AsyncCommand<FileItem> RenameFileCommand { get; set; }

        public Command ShareCommand { get; set; }

        public ObservableCollection<FileItem> FilesCollection { get; set; }

        public ObservableCollection<string> FilteredItems { get; set; }

        public FileItem? SelectedFile { get; set; }

        public List<string> Folders { get; set; }

        public string[] FilesLength { get; set; } = { "B", "KB", "MB", "GB", "TB" };

        public bool IsContextMenuOpen;

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        public FilesCollectionViewModel() {
            FilesCollection = new ObservableCollection<FileItem>();
            FilteredItems = new ObservableCollection<string>();
            TextToSearchCommand = new Command<string>(SearchAction);
            OpenFileCommand = new Command<FileItem>(OpenFileAction);
            ShareCommand = new Command<FileItem>(ShareAction);
            RenameFileCommand = new AsyncCommand<FileItem>(RenameFileActionAsync);

            Folders = new List<string> {
                ConstantsHelpers.TRANSLATIONS,
                ConstantsHelpers.TRANSCRIPTIONS,
                ConstantsHelpers.IMAGETEXT,
                ConstantsHelpers.AUDIOS
            };
            RetrieveFiles();
        }

        private async Task RenameFileActionAsync(FileItem file) {

            var name = Path.GetFileNameWithoutExtension(file.FilePath);
            var fileExt = Path.GetExtension(file.FilePath);
            var filePath = file.FilePath;

            var inputTextBox = new TextBox
            {
                AcceptsReturn = false, Text = name, VerticalAlignment = VerticalAlignment.Center
            };
            var dialog = new ContentDialog
            {
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

        private void ShareAction(FileItem file) {

            object mailClient = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Mail", "", "none");

            if (mailClient != null) {
                using (Process process = new Process()) {
                    process.StartInfo.UseShellExecute = true;
                    // You can start any process, HelloWorld is a do-nothing example.
                    process.StartInfo.FileName = mailClient.ToString();
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }

            }
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

                foreach (var folder in Folders) {

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
