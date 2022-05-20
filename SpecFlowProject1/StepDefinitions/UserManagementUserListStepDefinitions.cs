using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;

namespace TestingProject.StepDefinitions
{
    [Binding]
    public class UserManagementUserListStepDefinitions
    {
        private IWebDriver _driver;
        private string _HomePageUrl;
        private string _UserName;
        private string _Pwd;
        private IConfigurationRoot _configuration;

        [Given(@"The user launches the application")]
        public void GivenTheUserLaunchesTheApplication()
        {
            var general = new General();
            _configuration = general.GetConfigurationRoot();

            //load configuration data from appsettings.json
            var env = int.Parse(_configuration.GetSection("AppSettings").GetSection("RunEnvironment").Value ?? RunEnvironment.Local.ToString());
            switch (env)
            {
                case (int)RunEnvironment.Production:
                    _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlProd").Value;
                    _UserName = _configuration.GetSection("AppSettings").GetSection("UserProd").Value;
                    _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdProd").Value;
                    _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeProd").Value));
                    break;
                case (int)RunEnvironment.Staging:
                    _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlStaging").Value;
                    _UserName = _configuration.GetSection("AppSettings").GetSection("UserStaging").Value;
                    _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdStaging").Value;
                    _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeStaging").Value));
                    break;
                default:
                    _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlLocal").Value;
                    _UserName = _configuration.GetSection("AppSettings").GetSection("UserLocal").Value;
                    _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdLocal").Value;
                    _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeLocal").Value));
                    break;
            }

            //open the browser
            _driver.Navigate().GoToUrl(_HomePageUrl);

            //wait for companies are loaded (by calling api)
            var waitCompanyLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            waitCompanyLoad.Until(ExpectedConditions.ElementIsVisible(By.ClassName("ng-value-label")));
        }

        [Given(@"The user logins as admin with role = ""([^""]*)"" on login page")]
        public void GivenTheUserLoginsAsAdminWithRoleOnLoginPage(string delete)
        {
            throw new PendingStepException();
        }

        [Given(@"The user clicks on User Management page on the menu")]
        public void GivenTheUserClicksOnUserManagementPageOnTheMenu()
        {
            throw new PendingStepException();
        }

        [Given(@"The user is on the User list")]
        public void GivenTheUserIsOnTheUserList()
        {
            throw new PendingStepException();
        }

        [Then(@"The user sees elements on User list as the following table")]
        public void ThenTheUserSeesElementsOnUserListAsTheFollowingTable()
        {
            throw new PendingStepException();
        }
    }
}
