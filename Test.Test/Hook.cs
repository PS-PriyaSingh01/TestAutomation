using TechTalk.SpecFlow;
using System;
using System.IO;
using NUnit.Framework;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;

[assembly: Parallelizable(ParallelScope.Fixtures)]
namespace PowerSportsSupportS.Tests
{
    [Binding]
    public class HookInitialize
    {
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;

        private ExtentTest _currentScenarioName;
        private ExtentTest featureName;
        private static ExtentReports extent;

        public static ExtentKlovReporter klov;

        public HookInitialize(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext ?? throw new ArgumentNullException("featureContext");
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void Initialize()
        {
            //InitializeSettings(_scenarioContext.ScenarioInfo.Title);

            //Get feature Name
            featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title + " : " + _scenarioContext.ScenarioInfo.Title);

            //Create dynamic scenario name
            _currentScenarioName = featureName.CreateNode<Scenario>(" : " + _scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep]
        public void AfterEachStep()
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    _currentScenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
            }
            else if (_scenarioContext.TestError != null)
            {
                // Screenshot in the Base64 format
               // var mediaEntity = _parallelConfig.CaptureScreenshotAndReturnModel(_scenarioContext.ScenarioInfo.Title.Trim());

                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
            }
            else if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
            }
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {
            var dirPath = @"C:\extentreport\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(Path.Combine(dirPath, "TestReport.html"));
            // htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            // Attach report to reporter
            extent = new ExtentReports();

            #if RELEASE
            //if (TestContext.Parameters["ExecutionType"].ToString() == "Remote")
            //{
                klov = new ExtentKlovReporter();
                klov.InitMongoDbConnection("10.30.2.244", 27017);
                klov.ProjectName = "DS Test";
                //// URL of the KLOV server
                klov.InitKlovServerConnection("http://10.35.2.76:8080");
                klov.ReportName = "PSS Test" + DateTime.Now.ToString();
                extent.AttachReporter(htmlReporter, klov);
            //}
            //else extent.AttachReporter(htmlReporter);
            //#else
            //extent.AttachReporter(htmlReporter);
            #endif
        }


        [AfterTestRun]
        public static void TearDownReport()
        {
            extent.Flush();
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

