using TechTalk.SpecFlow;
using System;
using System.IO;
using NUnit.Framework;
using System.Xml.Linq;
using System.Linq;
using System.Xml;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace PowerSportsSupportS.Tests
{
    [Binding]
    public class HookInitialize
    {
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;

        public HookInitialize(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext ?? throw new ArgumentNullException("featureContext");
            _scenarioContext = scenarioContext;
        }

        [AfterStep]
        public void AfterEachStep()
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();            
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {
            //var dirPath = @"C:\extentreport\PSS\";
            //if (!Directory.Exists(dirPath))
            //{
            //    Directory.CreateDirectory(dirPath);
            
            //#if RELEASE
            //if (Settings.ExecutionType == "remote")
            //{
            //    klov = new ExtentKlovReporter();
            //    klov.InitMongoDbConnection("10.30.2.244", 27017);
            //    klov.ProjectName = "DS Test";
            //    //// URL of the KLOV server
            //    klov.InitKlovServerConnection("http://10.35.2.76:8080");
            //    klov.ReportName = "PSS Test" + DateTime.Now.ToString();
            //    extent.AttachReporter(htmlReporter, klov);
            //}
            //else extent.AttachReporter(htmlReporter);
            //#else
            //extent.AttachReporter(htmlReporter);
            //#endif
        }


        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report post test run completion
            //extent.Flush();

            //C:\ProgramData\Jenkins\.jenkins\workspace\TestSuite\Test.Test\bin\Release\netcoreapp3.1

            // Need to identify a way to identify the failed test post the publish report is generated - Or Not
            Console.WriteLine("Environment.CurrentDirectory" + Environment.CurrentDirectory);
            XmlDocument doc = new XmlDocument();


            //doc.Load("C:\\PSSAutomation\\Test.Automation\\Test.Test\\TestResults\\TestResults.xml");
            //try
            //{
                doc.Load(Path.Combine(Environment.CurrentDirectory,
                                @"..\..\..\TestResults\TestResults.xml"));
            //doc.Load("C:/ProgramData/Jenkins/.jenkins/workspace/TestSuite/Test.Test/TestResults/TestResults.xml");
                //XDocument doc = XDocument.Load("C:\\PSSAutomation\\Test.Automation\\Test.Test\\TestResults.xml");

                //string classnae = (string)doc.Root.Element("type");

                //string classnae = (string )doc.Root.Element("assembly/collection/test.type");
                //string classnae1 = (string)doc.Root.Element("assembly/collection/test.type.value");

                XmlNode node = doc.DocumentElement.FirstChild;

                string text = string.Empty;
                foreach (XmlNode n in node.ChildNodes)
                {
                    foreach (XmlNode n1 in n.ChildNodes)
                    {
                        if (n1.Attributes["result"]?.InnerText == "Fail")
                            text = n1.Attributes["type"]?.InnerText + "." + n1.Attributes["name"]?.InnerText; //or loop through its children as well
                    }
                }

                File.WriteAllText(Path.Combine(Environment.CurrentDirectory,
                                @"\ReRunTestResults.txt"), text);
            //}
            //catch
            //{
            //    Console.WriteLine("TestResults file doesn't exist");
            //}

            //var query = from c in doc.Document.Element.SelectSingleNode("assembly/collection/test")
            //            where (string)c.Attribute("result") == "Pass"
            //            select c.Element("type").Value;
        }
    }
}

