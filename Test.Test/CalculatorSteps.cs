using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Test.Steps
{
    [Binding]
    public class CalculatorSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CalculatorSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int firstnumber)
        {
            _scenarioContext["FirstNumber"] = firstnumber;
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int secondnumber)
        {
            _scenarioContext["SecondNumber"] = secondnumber;
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _scenarioContext["ActualResult"] = _scenarioContext.Get<int>("FirstNumber") + _scenarioContext.Get<int>("SecondNumber");
        }

        [When(@"the two numbers are substracted")]
        public void WhenTheTwoNumbersAreSubstracted()
        {
            _scenarioContext["ActualResult"] = _scenarioContext.Get<int>("FirstNumber") - _scenarioContext.Get<int>("SecondNumber");
        }

        [When(@"the two numbers are multiplied")]
        public void WhenTheTwoNumbersAreMultiplied()
        {
            _scenarioContext["ActualResult"] = _scenarioContext.Get<int>("FirstNumber") * _scenarioContext.Get<int>("SecondNumber");
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            Assert.AreEqual(result, _scenarioContext.Get<int>("ActualResult"));
        }
    }
}
