using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace TestingProject
{
    public class General
    {
        public IConfigurationRoot GetConfigurationRoot()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            return configurationBuilder.Build();
        }

        public IWebDriver SetBrowserDriver(int browserVal)
        {
            IWebDriver driver;
            switch (browserVal)
            {
                case (int)Browser.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver = new ChromeDriver();
                    break;
                case (int)Browser.FireFox:
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    driver = new FirefoxDriver();
                    break;
                case (int)Browser.Edge:
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    driver = new EdgeDriver();
                    break;
                default:
                    new DriverManager().SetUpDriver(new InternetExplorerConfig());
                    driver = new InternetExplorerDriver();
                    break;
            }

            return driver;
        }

    }

    //public enum RunEnvironment : sbyte
    //{
    //    Local = 1,
    //    Staging,
    //    Production
    //}

    public enum Browser : sbyte
    {
        Chrome = 1,
        FireFox,
        Edge,
        IE
    }

    public class UserType
    {
        public string Name { get; set; }
    }
}
