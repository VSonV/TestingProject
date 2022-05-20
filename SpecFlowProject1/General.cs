namespace TestingProject
{
    private static IConfigurationRoot GetConfigurationRoot()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
        return configurationBuilder.Build();
    }

    public enum RunEnvironment : sbyte
    {
        Local = 1,
        Staging,
        Production
    }

    public enum Browser : sbyte
    {
        Chrome = 1,
        FireFox,
        Edge,
        IE,
        Opera
    }
}
