namespace TranscribeMe.Services {
    public class FirebaseAuthService {

        private readonly IFirebaseAuthClient _firebaseAuthClient;

        public FirebaseAuthService(FirebaseAuthConfig firebaseAuthConfig) {

            _firebaseAuthClient = new FirebaseAuthClient(firebaseAuthConfig);
        }

        public async Task<UserCredential> RegisterAsync(string email, string password,
            string username) {

            return await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password,
                username);
        }

        public async Task<UserCredential> LoginAsync(string email, string password) {
            return await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
        }
    }
}
