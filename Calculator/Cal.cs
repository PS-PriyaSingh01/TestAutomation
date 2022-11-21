using System;
using System.IO;
using System.Xml;

namespace Calculator
{
    public class Cal
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }

        public void ReadTestResultFile()
        {
            Console.WriteLine("Environment.CurrentDirectory" + Environment.CurrentDirectory);
            //try
            //{
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(Environment.CurrentDirectory,
                                @"..\..\..\TestResults\TestResults.xml"));

            //doc.Load("http://localhost:8080/job/TestSuite/lastSuccessfulBuild/artifact/Test.Test/TestResults/TestResults.xml");

            doc.Load("$JENKINS_HOME/job/lastSuccessfulBuild/artifact/Test.Test/TestResults/TestResults.xml");

            XmlNode node = doc.DocumentElement.FirstChild;

            string text = "FullyQualifiedName=";
            foreach (XmlNode n in node.ChildNodes)
            {
                foreach (XmlNode n1 in n.ChildNodes)
                {
                    if (n1.Attributes["result"]?.InnerText == "Fail")
                        text = text + n1.Attributes["type"]?.InnerText + "." + n1.Attributes["name"]?.InnerText;
                }
            }

            File.WriteAllText(Path.Combine(Environment.CurrentDirectory,
                            @"..\..\..\ReRunTestResults.txt"), text);
            //}
            //catch(Exception ex)
            //{
            //Console.WriteLine(ex.Message);
            //Console.WriteLine(Path.Combine(Environment.CurrentDirectory,
            //                    @"..\..\..\TestResults\TestResults.xml"));
            //}
        }
    }
}
