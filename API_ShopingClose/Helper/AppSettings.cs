namespace API_ShopingClose.Helper
{
    public sealed class AppSettings
    {
        private static AppSettings _instance = null;
        private static readonly object padlock = new object();
        public readonly string _connectionString = string.Empty;

        private AppSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                .Build();
            _connectionString = configuration.GetSection("Tokentimes").GetSection("Tokenlifetimes").Value;
        }

        public static AppSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSettings();
                    }
                    return _instance;
                }
            }
        }

        public string ConnectionString
        {
            get => _connectionString;
        }
    }
}
