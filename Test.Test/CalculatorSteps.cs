﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TechTalk.SpecFlow;

namespace Test
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

        [Given(@"Divide a random number with 2")]
        public void GivenDivideARandomNumberWith()
        {
            Console.WriteLine("Environment.CurrentDirectory" + Environment.CurrentDirectory);
            int randomNumber = new Random().Next(2, 50);
            Console.WriteLine(randomNumber);
            _scenarioContext["ActualResult"] = randomNumber % 2;
        }


        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            Assert.AreEqual(result, _scenarioContext.Get<int>("ActualResult"));
        }

        [Then(@"Remainder should be (.*)")]
        public void ThenRemainderShouldBe(int expectedResult)
        {
            Assert.AreEqual(expectedResult, _scenarioContext.Get<int>("ActualResult"));
        }

        [Given(@"Read Test Result File")]
        public void GivenReadTestResultFile()
        {
            //IEnumerable<string> books = new string[] { Path.Combine(Environment.CurrentDirectory, @"..\..\..\TestResults\TestResults.xml"), Path.Combine(Environment.CurrentDirectory, @"..\..\..\\ReRunTestResults\TestResults.xml") };
           
            //NUnitMerger.MergeFiles(books, Path.Combine(Environment.CurrentDirectory, @"..\..\..\TestResults.xml"));

            var buildNumber = TestContext.Parameters["BuildNumber"].ToString();

            if (buildNumber.Contains("Deflake"))
            {
                
                XmlDocument doc = new XmlDocument();
                //doc.Load(Path.Combine(Environment.CurrentDirectory, @"..\..\..\TestResults\TestResults.xml"));

                string[] deflakedBuildNumber = buildNumber.Split(" ");
                string resultArtifactPath = "http://localhost:8080/job/TestSuite/" + deflakedBuildNumber[deflakedBuildNumber.Length-1].Replace("#", string.Empty) + "/artifact/Test.Test/TestResults/TestResults.xml";
                doc.Load(resultArtifactPath);

                // doc.Load("$JENKINS_HOME/job/lastSuccessfulBuild/artifact/Test.Test/TestResults/TestResults.xml");
                // doc.Load("$JENKINS_HOME/jobs//jobs//branches//builds/$BUILD_NUMBER/archive/");

                // Reding XUnit Test Result File
                XmlNode node = doc.DocumentElement.FirstChild;

                string text = string.Empty;
                foreach (XmlNode n in node.ChildNodes)
                {
                    foreach (XmlNode n1 in n.ChildNodes)
                    {
                        if (n1.Attributes["result"]?.InnerText == "Fail")
                            text = text + "FullyQualifiedName = " + n1.Attributes["type"]?.InnerText + "." + n1.Attributes["name"]?.InnerText + "|";
                    }
                }

                File.WriteAllText(Path.Combine(Environment.CurrentDirectory,
                                @"..\..\..\ReRunTestResults.txt"), text.Trim('|'));

                // Reding JUnit Test Result File
                //XmlNode node = doc.DocumentElement.FirstChild;

                //string text = string.Empty;
                //foreach (XmlNode n in node.ChildNodes)
                //{
                //    foreach (XmlNode n1 in n.ChildNodes)
                //    {
                //        if (n1.Attributes["result"]?.InnerText == "Fail")
                //            text = text + "FullyQualifiedName = " + n1.Attributes["type"]?.InnerText + "." + n1.Attributes["name"]?.InnerText + "|";
                //    }
                //}

                //File.WriteAllText(Path.Combine(Environment.CurrentDirectory,
                //                @"..\..\..\ReRunTestResults.txt"), text.Trim('|'));
            }
        }
    }
}
