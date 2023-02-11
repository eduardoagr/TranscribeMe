namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel {

        public string? UserId { get; set; }

        public string? DatabaseUrl { get; set; }

        public Command? SelectedPageCommand { get; set; }

        public NavButton? SelectedItem { get; set; }

        public MainWindowViewModel(string userID, string databaseUrl) {

            UserId = userID;
            DatabaseUrl = databaseUrl;
            SelectedPageCommand = new Command<ModernWpf.Controls.Frame>(PasgeSelectionItem);
        }

        private void PasgeSelectionItem(ModernWpf.Controls.Frame frame) {
            frame?.Navigate(SelectedItem?.NavLink);

            if (SelectedItem!.Name.Equals("CntactUS")) {
                SendEmail();
            }
            if (SelectedItem.Name.Equals("acc") && frame != null) {
                if (!string.IsNullOrEmpty(UserId)) {
                    if (!string.IsNullOrEmpty(UserId)) {
                        if (frame != null) {
                            SetLoadCompleted(frame);
                            frame.Navigate(SelectedItem.NavLink, new ProfilePageViewModel(UserId,
                                DatabaseUrl!));
                        }
                    }
                }
            }
        }

        private bool _setLoadCompleted = false;

        private void SetLoadCompleted(ModernWpf.Controls.Frame frame) {
            if (_setLoadCompleted) return;
            frame.LoadCompleted += (s, e) => {
                if (e.Content != null && e.ExtraData != null) (e.Content as Page).DataContext = e.ExtraData;
            };
            _setLoadCompleted = true;
        }

        private static void SendEmail() {

            var destinationurl = $"mailto:egomezr@outlook.com?Subject={Lang.IMPORTANT}&body=";
            var sInfo = new ProcessStartInfo(destinationurl) {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }
    }
}
