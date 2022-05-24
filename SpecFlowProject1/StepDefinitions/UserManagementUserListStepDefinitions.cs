using System.Text;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Interactions;

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
        private int _delayTime;

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
                    _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeProd").Value);
                    break;
                case (int)RunEnvironment.Staging:
                    _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlStaging").Value;
                    _UserName = _configuration.GetSection("AppSettings").GetSection("UserStaging").Value;
                    _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdStaging").Value;
                    _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeStaging").Value));
                    _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeStaging").Value);
                    break;
                default:
                    _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlLocal").Value;
                    _UserName = _configuration.GetSection("AppSettings").GetSection("UserLocal").Value;
                    _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdLocal").Value;
                    _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeLocal").Value));
                    _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeLocal").Value);
                    break;
            }

            //open the browser
            _driver.Navigate().GoToUrl(_HomePageUrl);
            _driver.Manage().Window.Maximize();

            //wait for home page is loaded
            var mainPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            mainPageLoad.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()=' Sign in ']")));
        }

        [Given(@"The user logins as admin with role = ""([^""]*)"" on login page")]
        public void GivenTheUserLoginsAsAdminWithRoleOnLoginPage(string delete)
        {
            var userNameId = _driver.FindElement(By.XPath("//label[text()='Username']"))?.GetAttribute("for");
            _driver.FindElement(By.Id(userNameId))?.SendKeys(_UserName);
            var passwordId = _driver.FindElement(By.XPath("//label[text()='Password']"))?.GetAttribute("for");
            _driver.FindElement(By.Id(passwordId))?.SendKeys(_Pwd);

            _driver.FindElement(By.XPath("//div[text()=' Sign in ']")).Click();
        }

        [Given(@"The user clicks on User Management page on the menu")]
        public void GivenTheUserClicksOnUserManagementPageOnTheMenu()
        {
            //wait for home page is loaded
            var mainPageLoad = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            mainPageLoad.Until(ExpectedConditions.ElementIsVisible(By.XPath("//img[@src='/img/LDP-LogoIcon.f9e7d727.png']")));
            var actions = new Actions(_driver);

            //hovor over leftside menu
            var mainMenu = _driver.FindElement(By.XPath("//img[@src='/img/LDP-LogoIcon.f9e7d727.png']"));
            actions.MoveToElement(mainMenu).Perform();

            //click on Accounts menu item
            _driver.FindElement(By.XPath("//li[@to='/accounts']"))?.Click();

            //click on User management menu item
            _driver.FindElement(By.XPath("//span[text()='User management']")).Click();
        }

        [Given(@"The user is on the User list")]
        public void GivenTheUserIsOnTheUserList()
        {
            var userManagementCls = _driver.FindElement(By.CssSelector("div[class='wrapper UserManagement-page']"));
            if (userManagementCls == null)
                throw new NotFoundException();
        }

        [Then(@"The user sees elements on User list as the following table")]
        public void ThenTheUserSeesElementsOnUserListAsTheFollowingTable()
        {

            var numberPerPageFilter = _driver.FindElement(By.Id("vs1__combobox"))?.Text;//number per page filter
            var userTypeFilter = _driver.FindElement(By.Id("vs4__combobox"))?.Text;//user type filter
            var statusFilter = _driver.FindElement(By.Id("vs5__combobox"))?.Text;//status filter
            var searchBar = _driver.FindElement(By.CssSelector("div[class='md-field input-search md-theme-default md-has-placeholder']"))?.Text;//search bar
            var addUserBtn = _driver.FindElement(By.XPath("//div[text()='Add user']"))?.Text;//button add user

            var mainTblHeader = _driver.FindElement(By.XPath("//div/thead/tr"))?.Text;
            var userType = mainTblHeader.Contains("User Type");//user type column
            var firstName = mainTblHeader.Contains("First Name");//first name
            var lastName = mainTblHeader.Contains("Last Name");//last name
            var uName = mainTblHeader.Contains("Username");//user name
            var email = mainTblHeader.Contains("Email");//email
            var pending = mainTblHeader.Contains("Pending");//pending
            var action = mainTblHeader.Contains("Action");//action

            var paging = _driver.FindElements(By.ClassName("pagination-wrapper"));//paging

        }

        [When(@"The user clicks on <column name> on User List")]
        public void WhenTheUserClicksOnColumnNameOnUserList()
        {
            //Thread.Sleep(_delayTime);
            //_driver.FindElement(By.XPath("//div[text()=' User Type ']"))?.Click();//user type column
            
            //Thread.Sleep(_delayTime);
            //_driver.FindElement(By.XPath("//div[text()=' First Name ']"))?.Click();//first name column

            //Thread.Sleep(_delayTime);
            //_driver.FindElement(By.XPath("//div[text()=' Last Name ']"))?.Click();//last name column

            //Thread.Sleep(_delayTime);
            //_driver.FindElement(By.XPath("//div[text()=' Username ']"))?.Click();//user name column

            Thread.Sleep(_delayTime);
            _driver.FindElement(By.XPath("//div[text()=' Email ']"))?.Click();//email column

            
        }

        [Then(@"the <column name> is sorted on User List")]
        public void ThenTheColumnNameIsSortedOnUserList()
        {
            var userTypeList = new StringBuilder();
            //get table rows
            var userTable = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr"));
            foreach(var tblRow in userTable)
            {
                var columns = tblRow.Text.Split("\r");
                for (int i = 0; i < columns.Length; i++)
                {
                    userTypeList.Append(i);
                }
            }
            var tmp = userTypeList.ToString();
        }

    }
}

