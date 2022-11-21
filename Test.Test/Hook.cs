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
            Console.WriteLine("Environment.CurrentDirectory" + Environment.CurrentDirectory);
            //try
            //{
            //XmlDocument doc = new XmlDocument();
            //doc.Load(Path.Combine(Environment.CurrentDirectory,
            //                    @"..\..\..\TestResults\TestResults.xml"));

            ////doc.Load("http://localhost:8080/job/TestSuite/lastSuccessfulBuild/artifact/Test.Test/TestResults/TestResults.xml");
            //// doc.Load("$JENKINS_HOME/job/lastSuccessfulBuild/artifact/Test.Test/TestResults/TestResults.xml");
            ////doc.Load("$JENKINS_HOME/jobs//jobs//branches//builds/$BUILD_NUMBER/archive/");

            //XmlNode node = doc.DocumentElement.FirstChild;

            //string text = "FullyQualifiedName=";
            //foreach (XmlNode n in node.ChildNodes)
            //{
            //    foreach (XmlNode n1 in n.ChildNodes)
            //    {
            //        if (n1.Attributes["result"]?.InnerText == "Fail")
            //            text = text + n1.Attributes["type"]?.InnerText + "." + n1.Attributes["name"]?.InnerText + "|";
            //    }
            //}

            //File.WriteAllText(Path.Combine(Environment.CurrentDirectory,
            //                @"..\..\..\ReRunTestResults.txt"), text.Trim('|'));
            ////}
            ////catch(Exception ex)
            ////{
            ////Console.WriteLine(ex.Message);
            ////Console.WriteLine(Path.Combine(Environment.CurrentDirectory,
            ////                    @"..\..\..\TestResults\TestResults.xml"));
            ////}         select c.Element("type").Value;
        }
    }
}

