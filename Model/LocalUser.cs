using MvvmHelpers;

namespace TranscribeMe.Model {
    public class LocalUser : BaseViewModel {
        private string? email;
        private string? username;
        private string? password;
        public string? confirm;

        public string Email {
            get => email!;
            set {
                email = value;
                OnPropertyChanged();
            }
        }
        public string Username {
            get => username!;
            set {
                username = value;
                OnPropertyChanged();
            }
        }
        public string Password {
            get => password!;
            set {
                password = value;
                OnPropertyChanged();
            }
        }

        public string Confirm {
            get => confirm!;
            set {
                confirm = value;
                OnPropertyChanged();
            }
        }
    }

}
