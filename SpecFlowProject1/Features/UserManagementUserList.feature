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

