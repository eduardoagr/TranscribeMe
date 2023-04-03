﻿using TranscribeMe.View.Dialogs;

namespace TranscribeMe.ViewModel.Pages {

    [AddINotifyPropertyChangedInterface]

    public class FileExplorerPageViewModel {


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

        public FileExplorerPageViewModel() {

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

            if (obj?.SelectedItem is FileItem item) {

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
        }

        private void GetFolders() {
            Folders = new List<string> {
                ConstantsHelpers.TRANSLATIONS,
                ConstantsHelpers.TRANSCRIPTIONS,
                ConstantsHelpers.IMAGETEXT,
                ConstantsHelpers.AUDIOS,
                ConstantsHelpers.VIDEOS
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
            if (file == null) { return; }

            // Check if the new file path already exists
            if (File.Exists(file!.FilePath)) {
                File.Delete(file.FilePath);
            }
            GetFiles();

        }

        private void PreviewAction(FileItem file) {
            if (file == null) { return; }

            if (Path.GetExtension(file.FilePath) == ".wmv" ||
                Path.GetExtension(file.FilePath) == ".mp4") {

                VideoPreviewDialog prev = new((file.FilePath));
                var mainWindow = Application.Current.MainWindow;
                double mainWindowLeft = mainWindow.Left;
                double mainWindowTop = mainWindow.Top;


                prev.Left = mainWindowLeft - prev.Width + 1200;
                prev.Top = mainWindowTop;


                prev.Show();

            } else {

                if (FileHelper.IsFileLocked(file)) {

                    FileHelper.CloseFileUsed(file);

                    var prevWindow = new PreviewWindow {
                        DataContext = new PreviewWindowViewModel(file)
                    };
                    prevWindow.Show();

                } else {

                    var prevWindow = new PreviewWindow {
                        DataContext = new PreviewWindowViewModel(file)
                    };
                    prevWindow.Show();
                }

            }
        }



        private async Task ShareActionAsync(FileItem item) {
            if (item == null) { return; }

            var value = item.FileLenght.Split(' ');

            if (Convert.ToDouble(value[0]) >= 10
                && value[1].Equals("MB")) {
                var dialog = new ContentDialog() {

                    Title = Lang.ErrorDialog,
                    Content = Lang.FileBig,
                    IsShadowEnabled = false,
                    PrimaryButtonText = Lang.Close,

                }.ShowAsync();
            } else {
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