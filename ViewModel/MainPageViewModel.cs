namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel {

        public Command SelectedPageCommand { get; set; }

        public NavButton? SelectedItem { get; set; }

        public MainPageViewModel() {

            SelectedPageCommand = new Command<ModernWpf.Controls.Frame>(PasgeSelectionItem);
        }

        private void PasgeSelectionItem(ModernWpf.Controls.Frame frame) {
            frame?.Navigate(SelectedItem?.NavLink);

            if (SelectedItem!.Name.Equals("CntactUS")) {

                SendEmail();
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
