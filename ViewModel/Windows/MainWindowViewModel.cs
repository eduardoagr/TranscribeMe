


namespace TranscribeMe.ViewModel.Windows {

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
                if (SelectedItem.Name.Equals("acc") && frame != null) {
                    frame.LoadCompleted += (sender, eventArgs) => {
                        if (eventArgs.Content is ProfilePage page) {
                            page.DataContext = new ProfilePageViewModel(
                                UserId!, DatabaseUrl!);
                        }
                    };
                    frame.Navigate(SelectedItem.NavLink);
                }
            }
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
