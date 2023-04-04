namespace TranscribeMe;

public partial class App : Application {

    private readonly IHost _host;

    private FirebaseAuthConfig? config;

    private string? DatabaeURL;

    public App() {

        //For debugging purposes

        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");


        SyncfusionLicenseProvider.RegisterLicense(ConstantsHelpers.SYNCFUSION_KEY);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {
                DatabaeURL = context.Configuration.GetValue<string>("DATABASEURL");
                string? FireDomain = context.Configuration.GetValue<string>("FIREBASE_DOMAIN");
                string? FireKey = context.Configuration.GetValue<string>("FIREBASE_KEY");

                config = new FirebaseAuthConfig() {

                    ApiKey = FireKey,
                    AuthDomain = FireDomain,
                    Providers = new FirebaseAuthProvider[] {
                      new EmailProvider()
                    }
                };

                services.AddSingleton((services) =>
                new SignUpLoginWondow(config, DatabaeURL!));

                // Register another window
                services.AddSingleton((services) => new MainWindow());
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {

        var uid = await LoginWithSavedDataAsync();

        if (!string.IsNullOrEmpty(uid)) {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = new MainWindowViewModel(uid!, DatabaeURL!);
        } else {
            MainWindow = _host.Services.GetRequiredService<SignUpLoginWondow>();
        }

        MainWindow.Show();
        base.OnStartup(e);
    }

    private async Task<string?> LoginWithSavedDataAsync() {
        string userDataFile = Path.Combine(
                        Environment.GetFolderPath
                        (Environment.SpecialFolder.LocalApplicationData), "userdata.json");

        if (File.Exists(userDataFile)) {
            var json = File.ReadAllText(userDataFile);
            var savedUser = JsonSerializer.Deserialize<UserData>(json);
            if (savedUser != null) {
                FirebaseAuthService firebaseAuth = new(config!);
                var res = await firebaseAuth.LoginAsync(savedUser.Object.Email,
                    savedUser.Object.Password);
                if (res.User != null) {
                    return res.User.Uid;
                }
            }
        }

        return null;
    }
}


