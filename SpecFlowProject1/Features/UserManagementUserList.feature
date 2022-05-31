Feature: User Management - UserList

A short summary of the feature

Background: 
	Given The user launches the application
	And The user logins as admin with role = "Delete" on login page
	And The user clicks on User Management page on the menu

Scenario: verify elements on user list
	Given The user is on the User list
	Then The user sees elements on User list as the following table

Scenario: verify sort function
	Given The user is on the User list
	When The user clicks on <column name> on User List
	Then the <column name> is sorted on User List

Scenario: verify UI of Number per page filter
	Given The user is on the User list
	When The user opens the filter Number Page
	Then The user sees items in the dropdown as following table: 25 per page | 50 per page

Scenario Outline: verify function of each item in Numbper per page filter
	Given The user is on the User list
	When The user opens the filter Number Page
	And The user selects a <number> in the Number Page filter
	Then The user sees the <number> of records are displayed on User List
	And The user sees the text "Showing 1 to" <number> "of [Total] entries" on User List

Examples: 
| number      |
| 25 per page |
| 50 per page |

Scenario: verify UI of User Type filter
	Given The user is on the User list
	When The user opens the filter User Type
	Then The user sees the items in the dropdown as the following table
| Name                |
| Administrator       |
| Clinic Associate    |
| Clinician           |
| Clinician Associate |
| Lab Technician      |
| Pathologist         |

Scenario Outline: verify fucntion of each item in User Type filter
	Given The user is on the User list
	When The user opens the filter User Type
	And The user selects an  <UserType> in the User Type filter
	Then The user sees only <UserType> records are displayed on User List

Examples: 
| UserType              |
| Administrator       |
| Clinic Associate    |
| Clinician           |
| Clinician Associate |
| Lab Technician      |
| Pathologist         |


Scenario: verify UI of Status filter
	Given The user is on the User list
	When The user open the Status filter 
	Then The user sees the items in the Status dropdown as the following table:
	| Name     |
	| Enabled  |
	| Disabled |
	| Invited  |

Scenario Outline: verify function of each item in Status filter
	Given The user is on the User list
	When The user open the Status filter
	And The user selects a <Status> in the Status filter
	Then The user sees only <Status> Status records filted are displayed on User List

Examples: 
| Status   |
| Enabled  |
| Disabled |
| Invited  |

Scenario Outline: verify function of Search bar
	Given The user is on the User list
	When The user inputs <data> into the search bar 
	Then The user sees only records filtered by <searchedBy> which meet the condition are displayed on User List <data>

Examples: 
| searchedBy | data             |
| First name | Anh              |
| First name | Tester           |
| First name | Alexander        |
| Last name  | Clinic Associate |
| Last name  | Gonchavar        |
| Last name  | kien             |
| Last name  | TC      |
| Email      | admin            |
| Email      | pushkin          |
| Email      | pushkin          |
| Email      | tcclinician      |

Scenario: check combine search
Given The user is on the User list
When The user inputs data as the following table:
| filter          | data          |
| Number per page | 25            |
| User type       | Administrator |
| Status          | Enabled       |
| Search bar      | Hà Anh        | #First name
Then the user sees only records which meet the condition are displayed on User List

Scenario: check if filter status of user list is saved after navigating to another page
Given The user is on the User list
When The user inputs data as the following table:
| filter          | data          |
| Number per page | 25            |
| User type       | Administrator |
| Status          | Enabled       |
| Search bar      | Hà Anh        | #First name
And the user edits an user record on User List
When the user clicks cancel button on the User form
And the user is redirected to the User List
Then the user sees filter status of user list is saved as before