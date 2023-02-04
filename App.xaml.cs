namespace TranscribeMe;

public partial class App : Application {

    private readonly IHost _host;
    public App() {

        //For debuging purposes

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");


        SyncfusionLicenseProvider.RegisterLicense(ConstantsHelpers.SYNCFUSION_KEY);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) => {

                string? FireDomain = context.Configuration.GetValue<string>("FIREBASE_DOMAIN");
                string? FireKey = context.Configuration.GetValue<string>("FIREBASE_KEY");

                var config = new FirebaseAuthConfig {
                    ApiKey = FireKey,
                    AuthDomain = FireDomain,
                    Providers = new FirebaseAuthProvider[] {
                      new EmailProvider()
                    }
                };

                services.AddSingleton<IFirebaseAuthClient>(new FirebaseAuthClient(config));
                var serviceProvider = services.BuildServiceProvider();
                var firebaseAuthClient = serviceProvider.GetRequiredService<IFirebaseAuthClient>();
                if (firebaseAuthClient == null) {
                    throw new InvalidOperationException("IFirebaseAuthClient is not registered with the container.");
                }
                services.AddSingleton((services) => new SignUpLoginWondow(firebaseAuthClient));
                // Register another window
                services.AddSingleton((services) => new MainWindow());
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e) {

        //if (LoginWithSavedData()) {
        //    MainWindow = _host.Services.GetRequiredService<MainWindow>();
        //} else {
        //    MainWindow = _host.Services.GetRequiredService<SignUpLoginWondow>();
        //}
        //MainWindow = _host.Services.GetRequiredService<SignUpLoginWondow>();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();

        MainWindow.Show();
        base.OnStartup(e);
    }

    private static bool LoginWithSavedData() {
        string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userdata.json");
        if (File.Exists(userDataFile)) {
            var savedUser = JsonSerializer.Deserialize<FireUser>(File.ReadAllText(userDataFile));
            if (!string.IsNullOrEmpty(savedUser!.Uid)) {
                return true;
            }
        }
        return false;
    }
}
