Feature: Calculator_Substract
	Simple calculator for substracting two numbers

@Calculator
Scenario: Simple calculator for substracting two numbers
	Given the first number is 80
	And the second number is 70
	When the two numbers are substracted
	Then the result should be 10