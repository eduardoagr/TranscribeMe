using MvvmHelpers;

namespace TranscribeMe.Model {

    public class LocalUser : ObservableObject {

        private string? firstName;
        private string? lastName;
        private string? password;
        private string? confirm;
        private string? country;
        private string? id;
        private string? email;
        private string? username;
        private string? city;
        private bool hasPaid;
        private bool isActive;
        private int age;
        private DateTime dateOfBirth;
        private DateTime suscriptionStart;
        private DateTime suscriptionEnd;
        readonly CultureInfo culture = CultureInfo.CurrentCulture;

        public LocalUser() {
        }

        public LocalUser(string? id, int age, string? username, string firstName,
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
            Age = age;
        }

        public DateTime DateOfBirth {
            get => dateOfBirth!;
            set {
                dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        public string DateOfBirthString {
            get {
                return dateOfBirth.ToString(culture.DateTimeFormat.ShortDatePattern);
            }
        }

        public string DateOfSuscriptionStartString {
            get {
                return suscriptionStart.ToString(culture.DateTimeFormat.ShortDatePattern);
            }
        }

        public string DateOfSuscriptionEndString {
            get {
                return suscriptionEnd.ToString(culture.DateTimeFormat.ShortDatePattern);
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
        public string? PhotoUrl { get; set; } = "https://transcribemedocs.blob.core.windows.net/default/icons8-customer-100.png";
        public int Age { get => age; set => age = value; }
    }

    public class UserData {
        public string Key { get; set; }
        public LocalUser Object { get; set; }
    }

}
