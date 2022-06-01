using System.Text;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
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
        private bool _IsValid = false;
        private int _rowsBefore=0;
        private int _rowsAfter=0;

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
            _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrl").Value;
            _UserName = _configuration.GetSection("AppSettings").GetSection("User").Value;
            _Pwd = _configuration.GetSection("AppSettings").GetSection("Pwd").Value;
            _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserType").Value));
            _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTime").Value);
            //var env = int.Parse(_configuration.GetSection("AppSettings").GetSection("RunEnvironment").Value ?? RunEnvironment.Local.ToString());
            //switch (env)
            //{
            //    case (int)RunEnvironment.Production:
            //        _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlProd").Value;
            //        _UserName = _configuration.GetSection("AppSettings").GetSection("UserProd").Value;
            //        _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdProd").Value;
            //        _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeProd").Value));
            //        _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeProd").Value);
            //        break;
            //    case (int)RunEnvironment.Staging:
            //        _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlStaging").Value;
            //        _UserName = _configuration.GetSection("AppSettings").GetSection("UserStaging").Value;
            //        _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdStaging").Value;
            //        _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeStaging").Value));
            //        _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeStaging").Value);
            //        break;
            //    default:
            //        _HomePageUrl = _configuration.GetSection("AppSettings").GetSection("HomeUrlLocal").Value;
            //        _UserName = _configuration.GetSection("AppSettings").GetSection("UserLocal").Value;
            //        _Pwd = _configuration.GetSection("AppSettings").GetSection("PwdLocal").Value;
            //        _driver = general.SetBrowserDriver(int.Parse(_configuration.GetSection("AppSettings").GetSection("BrowserTypeLocal").Value));
            //        _delayTime = int.Parse(_configuration.GetSection("AppSettings").GetSection("DeplayTimeLocal").Value);
            //        break;
            //}

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
            Thread.Sleep(_delayTime);
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

            _driver.Close();

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

            _driver.Close();
        }

        [When(@"The user opens the filter Number Page")]
        public void WhenTheUserOpensTheFilterNumberPage()
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

            _driver.Close();
        }

        [When(@"The user selects a (.*) per page in the Number Page filter")]
        public void WhenTheUserSelectsAPerPageInTheNumberPageFilter(int p0)
        {
            _driver.FindElement(By.XPath($"//*[text()='{p0} per page']"))?.Click();
        }

        [Then(@"The user sees the (.*) per page of records are displayed on User List")]
        public void ThenTheUserSeesThePerPageOfRecordsAreDisplayedOnUserList(int p0)
        {
            var userTableRows = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr"));
            Assert.IsTrue(userTableRows?.Count <= p0);
        }

        [Then(@"The user sees the text ""([^""]*)"" (.*) per page ""([^""]*)"" on User List")]
        public void ThenTheUserSeesTheTextPerPageOnUserList(string p0, int p1, string p2)
        {
            var userTableRows = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr")).Count;
            var searchStr = userTableRows <= p1 ? $"{p0} {userTableRows} of " : $"{p0} {p1} of";
            var pagingContent= _driver.FindElement(By.XPath("//div[@class='paginator']/div[@class='left-side']/p"))?.Text;
            Assert.IsTrue(pagingContent?.Contains(searchStr));

            _driver.Close();
        }

        [When(@"The user opens the filter User Type")]
        public void WhenTheUserOpensTheFilterUserType()
        {
            Thread.Sleep(_delayTime);
            _driver.FindElement(By.Id("vs4__combobox"))?.Click();
        }
        
        [Then(@"The user sees the items in the dropdown as the following table")]
        public void ThenTheUserSeesTheItemsInTheDropdownAsTheFollowingTable(Table table)
        {
            string tblRow = string.Join("", table.Rows.Select(x => x[0].ToString().Replace(" ", string.Empty)));

            var userTypeRows = _driver.FindElement(By.XPath("//ul[@id='vs4__listbox']"));
            string tmp = System.Text.RegularExpressions.Regex.Replace(userTypeRows.Text, @"\s+", string.Empty);

            Assert.AreEqual(tmp, tblRow);
            _driver.Close();
        }

        [When(@"The user selects an  (.*) in the User Type filter")]
        public void WhenTheUserSelectsAnInTheUserTypeFilter(string p0)
        {
            _driver.FindElement(By.XPath($"//*[text()='{p0}']"))?.Click();
        }

        [Then(@"The user sees only (.*) records are displayed on User List")]
        public void ThenTheUserSeesOnlyRecordsAreDisplayedOnUserList(string p0)
        {
            Thread.Sleep(_delayTime);
            var userTypeTbl = new StringBuilder();
            GetTableVal(ref userTypeTbl, 0);
            var result = userTypeTbl.ToString().Contains(p0);
            Assert.IsTrue(result);
            _driver.Close();
        }

        [When(@"The user open the Status filter")]
        public void WhenTheUserOpenTheStatusFilter()
        {
            Thread.Sleep(_delayTime);
            _driver.FindElement(By.Id("vs5__combobox"))?.Click();
        }


        [Then(@"The user sees the items in the Status dropdown as the following table:")]
        public void ThenTheUserSeesTheItemsInTheStatusDropdownAsTheFollowingTable(Table table)
        {
            Thread.Sleep(_delayTime);
            
            string tblRow = string.Join("", table.Rows.Select(x => x[0].ToString().Replace(" ", string.Empty)));

            var userTypeRows = _driver.FindElement(By.XPath("//ul[@id='vs5__listbox']"));
            string tmp = System.Text.RegularExpressions.Regex.Replace(userTypeRows.Text, @"\s+", string.Empty);

            Assert.AreEqual(tmp, tblRow);
            _driver.Close();
        }

        [When(@"The user selects a (.*) in the Status filter")]
        public void WhenTheUserSelectsAInTheStatusFilter(string p0)
        {
             _driver.FindElement(By.XPath($"//*[text()='{p0}']"))?.Click();
        }

        [Then(@"The user sees only (.*) Status records filted are displayed on User List")]
        public void ThenTheUserSeesOnlyStatusRecordsFiltedAreDisplayedOnUserList(string p0)
        {
            Thread.Sleep(_delayTime);
            var statusRows = _driver.FindElements(By.XPath("//td[@class='md-table-cell statusName-column']/div[@class='md-table-cell-container']/i"));
            foreach (var statusRow in statusRows)
            {
                Assert.IsTrue(statusRow.GetAttribute("class")?.Contains(p0.ToLower()));
            }
            _driver.Close();
        }


        [When(@"The user inputs (.*) into the search bar")]
        public void WhenTheUserInputsIntoTheSearchBar(string searchData)
        {
            Thread.Sleep(_delayTime);
            _driver.FindElement(By.XPath("//input[@placeholder='first name, last name, email, additional info']"))?.SendKeys(searchData);
        }

        [Then(@"The user sees only records filtered by (.*) which meet the condition are displayed on User List (.*)")]
        public void ThenTheUserSeesOnlyRecordsFilteredByFirstNameWhichMeetTheConditionAreDisplayedOnUserList(string searchedBy, string data)
        {
            var resultTbl = new StringBuilder();
            Thread.Sleep(_delayTime);
            if (searchedBy.Equals("First name"))
            {

                GetTableVal(ref resultTbl, 1);
            }
            else if (searchedBy.Equals("Last name"))
            {
                GetTableVal(ref resultTbl, 2);
            }
            else// email
            {
                GetTableVal(ref resultTbl, 4);
            }

            var filterVal = resultTbl.ToString().Contains(data, StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(filterVal);

            _driver.Close();
        }

        [When(@"The user inputs data as the following table:")]
        public void WhenTheUserInputsDataAsTheFollowingTable(Table table)
        {
            Thread.Sleep(_delayTime);

            var tmp = _driver.FindElement(By.XPath("//div[@class='md-card-content']"));
            new Actions(_driver).MoveToElement(tmp).Build().Perform();
            foreach (var row in table.Rows)
            {
                if (row["filter"].Contains("Number per page", StringComparison.OrdinalIgnoreCase))
                {
                    _driver.FindElement(By.Id("vs1__combobox"))?.Click();
                    _driver.FindElement(By.XPath($"//*[text()='{row["data"]} per page']"))?.Click();
                }
                else if (row["filter"].Contains("User type", StringComparison.OrdinalIgnoreCase))
                {
                    _driver.FindElement(By.Id("vs4__combobox"))?.Click();
                    _driver.FindElement(By.XPath($"//*[text()='{row["data"]}']"))?.Click();
                }
                else if (row["filter"].Contains("Status", StringComparison.OrdinalIgnoreCase))
                {
                    _driver.FindElement(By.Id("vs5__combobox"))?.Click();
                    _driver.FindElement(By.XPath($"//*[text()='{row["data"]}']"))?.Click();
                }
                else //search bar
                {
                    _driver.FindElement(By.XPath("//input[@placeholder='first name, last name, email, additional info']"))?.SendKeys(row["data"]);
                }
            }

            //checking result
            Thread.Sleep(_delayTime);
            var userTable = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody/tr"));

            foreach (var rw in userTable)
            {
                var validRow = table.Rows.Select(x => rw.Text.Contains(x["data"], StringComparison.OrdinalIgnoreCase)).ToList();
                _IsValid = validRow.Count>0;
            }
        }

        [Then(@"the user sees only records which meet the condition are displayed on User List")]
        public void ThenTheUserSeesOnlyRecordsWhichMeetTheConditionAreDisplayedOnUserList()
        {
            Assert.IsTrue(_IsValid);
            _driver.Close();
        }

        [When(@"the user edits an user record on User List")]
        public void WhenTheUserEditsAnUserRecordOnUserList()
        {
            var userTable = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody"));
            _rowsBefore = userTable.Count;
            if (_rowsBefore > 0)
            {
                //open edit user form
                userTable[0].Click();
                Thread.Sleep(_delayTime);

                //click edit button
                _driver.FindElement(By.XPath("//button[@class='md-button lims-form-button md-primary user-edit-btn md-theme-default']"))?.Click();
                Thread.Sleep(_delayTime);

                ////edit data
                //var telephoneElement =_driver.FindElement(By.XPath("//div[@class='md-field md-theme-default md-has-placeholder']/input[@class='md-input']"));
                //telephoneElement.Clear();
                //telephoneElement.SendKeys("123456789");
                //Thread.Sleep(_delayTime);
            }
        }

        [When(@"the user clicks cancel button on the User form")]
        public void WhenTheUserClicksCancelButtonOnTheUserForm()
        {
            //press cancel of edit action
            _driver.FindElement(By.XPath("//button[@class='md-button lims-form-button md-theme-default']"))?.Click();
            Thread.Sleep(_delayTime);

            ////click confirm unsave data
            //_driver.FindElement(By.XPath("//div[@class='md-dialog-actions']/button[@class='md-button lims-form-button md-primary md-theme-default']"))?.Click();

            //press cancel again to come back to previous screen
            _driver.FindElement(By.XPath("//button[@class='md-button lims-form-button md-theme-default']"))?.Click();
            Thread.Sleep(_delayTime);

           
        }

        [When(@"the user is redirected to the User List")]
        public void WhenTheUserIsRedirectedToTheUserList()
        {
            var userTable = _driver.FindElements(By.XPath("//div[@class='table-fix-header']/tbody"));
            _rowsAfter = userTable.Count;
            
        }

        [Then(@"the user sees filter status of user list is saved as before")]
        public void ThenTheUserSeesFilterStatusOfUserListIsSavedAsBefore()
        {
            Assert.IsTrue(_rowsAfter == _rowsBefore);
            _driver.Close();
        }

        [When(@"the user clicks button Add User on User List")]
        public void WhenTheUserClicksButtonAddUserOnUserList()
        {
            _driver.FindElement(By.XPath("//button[@class='md-button btn-addUser md-theme-default']")).Click();
        }

        [Then(@"the user sees a dialog ""([^""]*)"" with elements as following:")]
        public void ThenTheUserSeesADialogWithElementsAsFollowing(string p0, Table table)
        {
            _driver.FindElement(By.XPath("//div[@class='vs__selected-options']"));
            _driver.FindElement(By.XPath("//button[@class='md-button lims-form-button md-theme-default']"));
            _driver.FindElement(By.XPath("//button[@class='md-button lims-form-button md-primary md-theme-default']"));
            _driver.FindElement(By.XPath("//i[@class='md-icon md-icon-font md-theme-default']"));
            _driver.Close();
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

