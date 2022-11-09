Feature: Calculator_Add
	Simple calculator for adding two numbers

@Calculator
Scenario: Simple calculator for adding two numbers
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120