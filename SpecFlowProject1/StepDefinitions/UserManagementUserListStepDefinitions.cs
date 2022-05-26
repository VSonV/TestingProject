using System.Text;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;

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

        StringBuilder userTypeBefore;
        StringBuilder firstNameBefore;
        StringBuilder lastNameBefore;
        StringBuilder userNameBefore;
        StringBuilder emailNameBefore;
        StringBuilder userTypeAfter;
        StringBuilder firstNameAfter;
        StringBuilder lastNameAfter;
        StringBuilder userNameAfter;
        StringBuilder emailNameAfter;

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
            Thread.Sleep(_delayTime);
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

            _driver.Quit();

        }

        [When(@"The user clicks on <column name> on User List")]
        public void WhenTheUserClicksOnColumnNameOnUserList()
        {
            //get user type value before and after click event
            Thread.Sleep(_delayTime);
            userTypeBefore = new StringBuilder();
            GetTableVal(ref userTypeBefore,0);
            _driver.FindElement(By.XPath("//div[text()=' User Type ']"))?.Click();//user type column
            Thread.Sleep(_delayTime);
            userTypeAfter = new StringBuilder();
            GetTableVal(ref userTypeAfter, 0);

            //get first name value before and after click event
            Thread.Sleep(_delayTime);
            firstNameBefore = new StringBuilder();
            GetTableVal(ref firstNameBefore, 1);
            _driver.FindElement(By.XPath("//div[text()=' First Name ']"))?.Click();//first name column
            Thread.Sleep(_delayTime);
            firstNameAfter = new StringBuilder();
            GetTableVal(ref firstNameAfter, 1);

            Thread.Sleep(_delayTime);
            lastNameBefore = new StringBuilder();
            GetTableVal(ref lastNameBefore, 2);
            _driver.FindElement(By.XPath("//div[text()=' Last Name ']"))?.Click();//last name column
            Thread.Sleep(_delayTime);
            lastNameAfter = new StringBuilder();
            GetTableVal(ref lastNameAfter, 2);

            Thread.Sleep(_delayTime);
            userNameBefore = new StringBuilder();
            GetTableVal(ref userNameBefore, 3);
            _driver.FindElement(By.XPath("//div[text()=' Username ']"))?.Click();//user name column
            Thread.Sleep(_delayTime);
            userNameAfter = new StringBuilder();
            GetTableVal(ref userNameAfter, 3);

            Thread.Sleep(_delayTime);
            emailNameBefore = new StringBuilder();
            GetTableVal(ref emailNameBefore, 4);
            _driver.FindElement(By.XPath("//div[text()=' Email ']"))?.Click();//email column
            Thread.Sleep(_delayTime);
            emailNameAfter = new StringBuilder();
            GetTableVal(ref emailNameAfter, 4);
        }

        [Then(@"the <column name> is sorted on User List")]
        public void ThenTheColumnNameIsSortedOnUserList()
        {
            var userTypeValBefore = userTypeBefore.ToString();
            var userTypeValAfter = userTypeAfter.ToString();
            Assert.IsFalse(userTypeValBefore.Equals(userTypeValAfter));

            var firstNameValBefore = firstNameBefore.ToString();
            var firstNameValAfter = firstNameAfter.ToString();
            Assert.IsFalse(firstNameValBefore.Equals(firstNameValAfter));

            var lastNameValBefore = lastNameBefore.ToString();
            var lastNameValAfter = lastNameAfter.ToString();
            Assert.IsFalse(lastNameValBefore.Equals(lastNameValAfter));

            var uNameValBefore = userNameBefore.ToString();
            var uNameValAfter = userNameAfter.ToString();
            Assert.IsFalse(uNameValBefore.Equals(uNameValAfter));

            var emailValBefore = emailNameBefore.ToString();
            var emailValAfter = emailNameAfter.ToString();
            Assert.IsFalse(emailValBefore.Equals(emailValAfter));

            userTypeBefore.Clear();
            userTypeAfter.Clear();
            firstNameBefore.Clear();
            firstNameAfter.Clear();
            lastNameBefore.Clear();
            lastNameAfter.Clear();
            userNameBefore.Clear();
            userNameAfter.Clear();
            emailNameBefore.Clear();
            emailNameAfter.Clear();

            _driver.Quit();
        }

        [When(@"The user opens the filter ""([^""]*)""")]
        public void WhenTheUserOpensTheFilter(string p0)
        {
            Thread.Sleep(_delayTime); 
            _driver.FindElement(By.Id("vs1__combobox"))?.Click();
        }

        [Then(@"The user sees items in the dropdown as following table: (.*) per page \| (.*) per page")]
        public void ThenTheUserSeesItemsInTheDropdownAsFollowingTablePerPagePerPage(int p0, int p1)
        {
            Thread.Sleep(_delayTime);
            var itemPerPageList = _driver.FindElement(By.Id("vs1__listbox")).Text;
            Assert.IsTrue(itemPerPageList.Contains(p0.ToString()) && itemPerPageList.Contains(p1.ToString()));

            _driver.Quit();
        }

        [When(@"The user selects a (.*) per page in the filter")]
        public void WhenTheUserSelectsAPerPageInTheFilter(int p0)
        {
            _driver.FindElement(By.XPath($"//*[text()='{p0} per page']"))?.Click();
        }

        [Then(@"The user sees the (.*) per page of records are displayed on User List")]
        public void ThenTheUserSeesThePerPageOfRecordsAreDisplayedOnUserList(int p0)
        {
            Thread.Sleep(_delayTime);
            var userTableRows = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr"));
            Assert.IsTrue(userTableRows?.Count <= p0);
        }

        [Then(@"The user sees the text ""([^""]*)"" (.*) per page ""([^""]*)"" on User List")]
        public void ThenTheUserSeesTheTextPerPageOnUserList(string p0, int p1, string p2)
        {
            Thread.Sleep(_delayTime);
            var userTableRows = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr")).Count;
            var searchStr = userTableRows <= p1 ? $"{p0} {userTableRows} of " : $"{p0} {p1} of";
            var pagingContent= _driver.FindElement(By.XPath("//div[@class='paginator']/div[@class='left-side']/p"))?.Text;
            Assert.IsTrue(pagingContent?.Contains(searchStr));
        }

        private void GetTableVal(ref StringBuilder fieldVal, int colPosition)
        {
            var userTable = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr"));
            foreach (var tblRow in userTable)
            {
                var columns = tblRow.Text.Split("\r");
                for (int i = 0; i < columns.Length; i++)
                {
                    if (i == colPosition)
                    {
                        fieldVal.Append(columns[i]);
                        break;
                    }
                }
            }

        }
    }
}

