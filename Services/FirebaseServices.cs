namespace TranscribeMe.Services {
    public class FirebaseServices {

        private readonly FirebaseClient _firebaseClient;

        public FirebaseServices(string databaseUrl) {
            _firebaseClient = new FirebaseClient(databaseUrl);
        }

        public async Task<FirebaseObject<LocalUser>?> GetAsync
            (string Node, string id) {

            var result = await _firebaseClient
                .Child(Node)
                .OnceAsync<LocalUser>();

            var currentUser = result.Where
                (u => u.Object.Id == id)
                .FirstOrDefault();

            return currentUser;
        }

        public async Task<IReadOnlyCollection<FirebaseObject<LocalUser>>> GetAllAsync(string Node) {

            var FireUsers = await _firebaseClient
               .Child(Node)
               .OnceAsync<LocalUser>();

            return FireUsers;

        }

        public async Task<FirebaseObject<LocalUser>> CreateAsync(string Node, LocalUser user) {

            var newUser = await _firebaseClient.Child(Node)
            .PostAsync(user);

            return newUser;
        }



        public async Task UpdateAsync(string Node, string key,
            FirebaseObject<LocalUser> firebaseObject) {

            await _firebaseClient.Child(Node)
                .Child(key).PatchAsync(firebaseObject.Object);
        }

        public async Task DeleteAsync(string path) {
            await _firebaseClient.Child(path).DeleteAsync();
        }
    }

}
