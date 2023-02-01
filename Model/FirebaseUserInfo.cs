namespace TranscribeMe.Model {
    public class Info {
        public string Uid { get; set; }
        public object FederatedId { get; set; }
        public object FirstName { get; set; }
        public object LastName { get; set; }
        public object DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public object PhotoUrl { get; set; }
        public bool IsAnonymous { get; set; }
    }

    public class Credential {
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Created { get; set; }
        public int ExpiresIn { get; set; }
        public int ProviderType { get; set; }
    }

    public class FireUser {
        public string Uid { get; set; }
        public bool IsAnonymous { get; set; }
        public Info Info { get; set; }
        public Credential Credential { get; set; }
    }
}

