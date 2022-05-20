using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;

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
                    driver = new ChromeDriver(System.Environment.CurrentDirectory);
                    break;
                case (int)Browser.FireFox:
                    driver = new FirefoxDriver(System.Environment.CurrentDirectory);
                    break;
                case (int)Browser.Edge:
                    driver = new EdgeDriver(System.Environment.CurrentDirectory);
                    break;
                case (int)Browser.IE:
                    driver = new InternetExplorerDriver(System.Environment.CurrentDirectory);
                    break;
                default:
                    driver = new OperaDriver(System.Environment.CurrentDirectory);
                    break;
            }

            return driver;
        }

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
