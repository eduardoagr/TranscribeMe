using Fonts;

using System.Windows.Threading;

namespace TranscribeMe.ViewModel.Dialogs {

    [AddINotifyPropertyChangedInterface]
    public class PreviewDialogViewModel {

        private MediaElement mediaElement;

        public string? Content { get; set; }

        private DispatcherTimer _timer;

        public Uri Source { get; set; }

        public int CurrentValue { get; set; }

        public bool IsPlaying { get; set; } = true;

        public int MaxValue { get; set; }

        public Command<MediaElement> MediaOpenedCommand { get; set; }

        public Command<MediaElement> PlayPauseCommand { get; set; }

        public Command<MediaElement> SeekToMediaPositionCommand { get; set; }

        public PreviewDialogViewModel() {


        }

        public PreviewDialogViewModel(Uri uri) {

            Source = uri;
            Content = IconFont.Play;
            IsPlaying = true;
            MediaOpenedCommand = new Command<MediaElement>(MediaOpenedAction);
            PlayPauseCommand = new Command<MediaElement>(PlayPauseAction);
            SeekToMediaPositionCommand = new Command<MediaElement>(SeekToMediaPositionAction);

            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e) {
            if (mediaElement != null) {
                int currentValue = (int)mediaElement.Position.TotalMilliseconds;
                Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;
                currentDispatcher.BeginInvoke((Action)(() => {
                    CurrentValue = currentValue;
                }));
            }
        }

        private void MediaOpenedAction(MediaElement media) {

            mediaElement = media;
            MaxValue = (int)media.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void SeekToMediaPositionAction(MediaElement media) {

            int SliderValue = CurrentValue;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            media.Position = ts;
        }

        private void PlayPauseAction(MediaElement obj) {

            if (IsPlaying) {

                Content = IconFont.Pause;

            } else {

                Content = IconFont.Play;

            }

            IsPlaying = !IsPlaying;
        }
    }
}
