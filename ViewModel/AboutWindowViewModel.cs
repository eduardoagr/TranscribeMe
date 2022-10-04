namespace TranscribeMe.ViewModel {
    public class AboutWindowViewModel {

        public Command<Window> CloseWindowCommand { get; set; }

        public AboutWindowViewModel() {

            CloseWindowCommand = new Command<Window>(CloseWindowCommandAction);
        }

        private void CloseWindowCommandAction(Window obj) {

            obj.Close();
        }
    }
}
