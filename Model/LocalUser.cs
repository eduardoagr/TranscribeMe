

using MvvmHelpers;

namespace TranscribeMe.Model {

    public class LocalUser : ObservableObject {

        private string? firstName;
        private string? lastName;
        private string? photoUrl;
        private string? password;
        private string? confirm;
        private string? country;
        private string? id;
        private string? email;
        private string? username;
        private string? city;
        private bool hasPaid;
        private bool isActive;
        private DateTime dateOfBirth = new DateTime(2018, 05, 05);
        private DateTime suscriptionStart;
        private DateTime suscriptionEnd;


        public LocalUser() {
        }

        public LocalUser(string? id, string? username, string firstName,
            string? lastName, string? photoUrl,
            string? email, string? country, string? city, bool hasPaid,
            bool isActive, DateTime dateOfBirth,
            DateTime suscriptionStartDate, DateTime suscriptionEndDate) {

            FirstName = firstName;
            LastName = lastName;
            PhotoUrl = photoUrl;
            Country = country;
            Id = id!;
            DateOfBirth = dateOfBirth;
            Email = email!;
            Username = username!;
            City = city;
            HasPaid = hasPaid;
            SuscriptionStartDate = suscriptionStartDate;
            SuscriptionEndDate = suscriptionEndDate;
            IsActive = isActive;
        }

        public DateTime DateOfBirth {
            get => dateOfBirth!;
            set {
                dateOfBirth = value;
                OnPropertyChanged();
            }
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

        public string? City { get => city; set => city = value; }
        public bool HasPaid { get => hasPaid; set => hasPaid = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public DateTime SuscriptionStartDate { get => suscriptionStart; set => suscriptionStart = value; }
        public DateTime SuscriptionEndDate { get => suscriptionEnd; set => suscriptionEnd = value; }
        public string? Country { get => country; set => country = value; }
        public string? FirstName { get => firstName; set => firstName = value; }
        public string? LastName { get => lastName; set => lastName = value; }
        public string? PhotoUrl { get => photoUrl; set => photoUrl = value; }
    }

    public class UserData {
        public string Key { get; set; }
        public LocalUser Object { get; set; }
    }

}
