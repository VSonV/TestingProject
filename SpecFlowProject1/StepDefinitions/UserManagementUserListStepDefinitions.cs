using System;
using TechTalk.SpecFlow;

namespace TestingProject.StepDefinitions
{
    [Binding]
    public class UserManagementUserListStepDefinitions
    {
        [When(@"The user clicks on <column name> on User List")]
        public void WhenTheUserClicksOnColumnNameOnUserList()
        {
            throw new PendingStepException();
        }

        [Then(@"the <column name> is sorted on User List")]
        public void ThenTheColumnNameIsSortedOnUserList()
        {
            throw new PendingStepException();
        }
    }
}
