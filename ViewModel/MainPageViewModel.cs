using ModernWpf.Controls.Primitives;

using TranscribeMe.CustomControls;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel {

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation,
        string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public Command SelectedPageCommand { get; set; }

        public NavButton? SelectedItem { get; set; }

        public MainPageViewModel() {

            SelectedPageCommand = new Command<ModernWpf.Controls.Frame>(PasgeSelectionItem);
        }

        private async void PasgeSelectionItem(ModernWpf.Controls.Frame frame) {
            frame?.Navigate(SelectedItem?.NavLink);

            if (SelectedItem!.Name.Equals("CntactUS")) {
                StackPanel stackPanel = new();
                Grid richTextBoxGrid = new();
                TextBox textBox = new();
                RichTextBox richTextBox = new()
                {
                    Height = 200,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                ControlHelper.SetPlaceholderText(textBox, "Email address");

                ControlHelper.SetPlaceholderText(richTextBox, "Message to the developer:" +
                                " \n * This could be a problem you encounter using the app " +
                                " \n * something that you would like to see in future versions");

                richTextBoxGrid.Children.Add(richTextBox);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(richTextBoxGrid);

                var dialog = new ContentDialog()
                {
                    Content = stackPanel,
                    Title = "Contact support",
                    IsSecondaryButtonEnabled = true,
                    PrimaryButtonText = "Send",
                    SecondaryButtonText = "Cancel"
                };

                ContentDialogResult result = await dialog.ShowAsync();

                // Delete the file if the user clicked the primary button.
                /// Otherwise, do nothing.
                if (result == ContentDialogResult.Primary) {
                    SendEmail();
                }
            }
        }

        private static void SendEmail() {

            string mailto = $"mailto:egomezr@outlook.com?Subject=Problems&Body=";
            ShellExecute(IntPtr.Zero, "open", mailto,

            "", "", 4 /* sw_shownoactivate */);



        }
    }
}
