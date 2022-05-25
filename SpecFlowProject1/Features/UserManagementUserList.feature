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
	When The user opens the filter "Number Page"
	Then The user sees items in the dropdown as following table: 25 per page | 50 per page

