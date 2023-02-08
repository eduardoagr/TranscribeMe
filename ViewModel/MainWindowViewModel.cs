using TranscribeMe.Pages;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel {

        public string UserId { get; set; }

        public Command SelectedPageCommand { get; set; }

        public NavButton? SelectedItem { get; set; }

        public MainWindowViewModel(string userID) {

            UserId = userID;

            SelectedPageCommand = new Command<ModernWpf.Controls.Frame>(PasgeSelectionItem);
        }

        private void PasgeSelectionItem(ModernWpf.Controls.Frame frame) {
            frame?.Navigate(SelectedItem?.NavLink);

            if (SelectedItem!.Name.Equals("CntactUS")) {
                SendEmail();
            }
            if (SelectedItem.Name.Equals("acc")) {
                if (!string.IsNullOrEmpty(UserId)) {
                    if (frame != null) {
                        frame.LoadCompleted += (sender, eventArgs) => {
                            if (eventArgs.Content is ProfilePage page) {
                                page.DataContext = new ProfilePageViewModel(UserId);
                            }
                        };
                        frame.Navigate(SelectedItem.NavLink);
                    }
                }
            }
        }

        private static void SendEmail() {

            var destinationurl = $"mailto:egomezr@outlook.com?Subject=IMPORTANT&body=";
            var sInfo = new ProcessStartInfo(destinationurl) {
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Minimized
            };
            Process.Start(sInfo);
        }
    }
}
