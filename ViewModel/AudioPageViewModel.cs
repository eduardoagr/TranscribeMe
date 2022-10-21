using TranscribeMe.Helpers;

namespace TranscribeMe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class AudioPageViewModel {

        public string? FilePath { get; set; }

        public bool IsWorking { get; set; }

        public List<string>? Languages { get; set; }

        public Visibility CanShow { get; set; }

        public DialogHelper Dialog { get; }

        public Command PickFileCommad { get; set; }

        public Command StartCommand { get; set; }

        private string? _SelectedItem;

        public string SelectedItem {
            get => _SelectedItem!;
            set {
                if (_SelectedItem != value) {
                    _SelectedItem = value;
                    StartCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public AudioPageViewModel() {
            InitListLanguages();
            Dialog = new DialogHelper();
            CanShow = Visibility.Hidden;
            PickFileCommad = new Command(PickFileAction);
            StartCommand = new Command(StartAction, CanStartAction);
        }

        private bool CanStartAction(object arg) {
            if (string.IsNullOrEmpty(SelectedItem) ||
                string.IsNullOrEmpty(FilePath)) {
                return false;
            }
            return true;
        }

        private void StartAction(object obj) {
        }

        private void PickFileAction() {
            var filePath = Dialog.GetFilePath(ConstantsHelpers.AUDIO);
            FilePath = filePath;
        }

        private void InitListLanguages() {

            Languages = new List<string>() {
                "English",
                "Spanish"
            };
        }
    }
}
