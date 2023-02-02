

using MvvmHelpers;

namespace TranscribeMe.Model {

    public class LocalUser : ObservableObject {

        private string? password;
        public string? confirm;
        public string? displayName;
        public string? country;
        private string? id;
        private string? email;
        private string? username;
        public string? city;
        public bool hasPaid;

        public LocalUser() {
        }

        public LocalUser(string? id, string? email, string? username,
            string? city, string? country, bool hasPaid) {

            this.id = id;
            this.email = email;
            this.username = username;
            this.city = city;
            this.hasPaid = hasPaid;
            this.country = country;
        }

        public string Id {
            get => id!;
            set {
                id = value;
                OnPropertyChanged();
            }
        }

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
