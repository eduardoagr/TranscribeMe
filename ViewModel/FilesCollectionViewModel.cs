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

        public List<string>? Folders { get; set; }

        public string[] FilesLength { get; set; } = { "B", "KB", "MB", "GB", "TB" };

        public bool IsContextMenuOpen;

        #region HRESULT 

        public enum HRESULT : int {
            S_OK = 0,
            S_FALSE = 1,
            E_NOINTERFACE = unchecked((int)0x80004002),
            E_NOTIMPL = unchecked((int)0x80004001),
            E_FAIL = unchecked((int)0x80004005),
        }

        #endregion

        #region comImport

        [ComImport]
        [Guid("00000122-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IDropTarget {
            HRESULT DragEnter(
                [In] System.Runtime.InteropServices.ComTypes.IDataObject pDataObj,
                [In] int grfKeyState,
                [In] System.Windows.Point pt,
                [In, Out] ref int pdwEffect);

            HRESULT DragOver(
                [In] int grfKeyState,
                [In] System.Windows.Point pt,
                [In, Out] ref int pdwEffect);

            HRESULT DragLeave();

            HRESULT Drop(
                [In] System.Runtime.InteropServices.ComTypes.IDataObject pDataObj,
                [In] int grfKeyState,
                [In] System.Windows.Point pt,
                [In, Out] ref int pdwEffect);
        }
        #endregion

        #region DLLs

        public const int DROPEFFECT_NONE = (0);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "#740")]
        public static extern HRESULT SHCreateFileDataObject(IntPtr pidlFolder, uint cidl, IntPtr[] apidl, System.Runtime.InteropServices.ComTypes.IDataObject pdtInner, out System.Runtime.InteropServices.ComTypes.IDataObject ppdtobj);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern HRESULT SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr ILFindLastID(IntPtr pidl);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr ILClone(IntPtr pidl);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean ILRemoveLastID(IntPtr pidl);

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void ILFree(IntPtr pidl);

        Guid CLSID_MapiMail = new Guid("9E56BE60-C50F-11CF-9A2C-00A0C90A90CE");

        #endregion

        public FilesCollectionViewModel() {
            FilesCollection = new ObservableCollection<FileItem>();
            FilteredItems = new ObservableCollection<string>();
            TextToSearchCommand = new Command<string>(SearchAction);
            OpenFileCommand = new Command<FileItem>(OpenFileAction);
            ShareCommand = new Command<FileItem>(ShareAction);
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
        private async void ShareAction(FileItem file) {

            AzureStorageService azureStorageService = new();

            var link = await azureStorageService.UploadToAzureBlobStorage(file.FilePath);

            string subject = "Here is the link for your file";

            var destinationurl = $"mailto:?subject={subject}&body={link}%0D";
            var sInfo = new ProcessStartInfo(destinationurl) {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
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