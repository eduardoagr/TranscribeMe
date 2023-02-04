

using MvvmHelpers;

namespace TranscribeMe.Model {

    public class LocalUser : ObservableObject {

        private string? password;
        private string? confirm;
        public string? country;
        private string? id;
        private string? email;
        private string? username;
        public string? city;
        public bool hasPaid;
        public bool isActive;
        public DateTime? suscriptionStart;
        public DateTime? suscriptionEnd;


        public LocalUser() {
        }

        public LocalUser(string? country, string? id,
            string? email, string? username, string? city, bool hasPaid,
            DateTime? suscriptionStart, DateTime? suscriptionEnd, bool isActive) {

            this.country = country;
            this.id = id;
            this.email = email;
            this.username = username;
            this.city = city;
            this.hasPaid = hasPaid;
            this.suscriptionStart = suscriptionStart;
            this.suscriptionEnd = suscriptionEnd;
            this.isActive = isActive;
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
