namespace SMSMicroService.Helpers
{
    public class AppConfig
    {
        public static string Get(string key)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            return config[key];
        }
    }
}
