Feature: Calculator_Multiply
	Simple calculator for multiplying two numbers

@Calculator
Scenario: Simple calculator for multiplying two numbers
	Given the first number is 10
	And the second number is 5
	When the two numbers are multiplied
	Then the result should be 50