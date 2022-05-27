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
| Name     |
| Enabled  |
| Disabled |
| Invited  |
