using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TRXFileM
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0
                || args.Contains("/h")
                || args.Contains("/help"))
            {
                //DispalyHelp();
                return 1;
            }

            if (args.Where(a => a.StartsWith("/trx")).FirstOrDefault() == null)
            {
                Console.WriteLine("/trx parameter is required");
                return 1;
            }

            string trxArg = args.Where(a => a.StartsWith("/trx")).FirstOrDefault();
            var trxFiles = ResolveTrxFilePaths(trxArg, args.Contains("/r"));
            if (trxFiles.Count == 0)
            {
                Console.WriteLine("No trx files found!");
                return 1;
            }

            if (trxFiles.Count == 1)
            {
                if (trxFiles[0].StartsWith("Error: "))
                {
                    Console.WriteLine(trxFiles[0]);
                    return 1;
                }

                if (args.Where(a => a.StartsWith("/report")).FirstOrDefault() == null)
                {
                    Console.WriteLine("Error: Only one trx file has been passed and there is no /report parameter. When having only one trx in /trx argument, /report parameter is required.");
                    return 1;
                }

                if (args.Where(a => a.StartsWith("/output")).FirstOrDefault() != null)
                {
                    Console.WriteLine("Error: /output parameter is not allowed when having only one trx in /trx argument!.");
                    return 1;
                }
            }
            else
            {
                if (args.Where(a => a.StartsWith("/output")).FirstOrDefault() == null)
                {
                    Console.WriteLine("/output parameter is required, when there are multiple trx files in /trx argument");
                    return 1;
                }

                string outputParam = ResolveOutputFileName(args.Where(a => a.StartsWith("/output")).FirstOrDefault());
                if (outputParam.StartsWith("Error: "))
                {
                    Console.WriteLine(outputParam);
                    return 1;
                }

                if (trxFiles.Contains(outputParam))
                    trxFiles.Remove(outputParam);

                try
                {
                    var combinedTestRun = TestRunMerger.MergeTRXsAndSave(trxFiles, outputParam);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    Console.WriteLine("Error: " + ex.Message);
                    return 1;
                }
            }

            return 0;
        }

        private static string ResolveOutputFileName(string outputParam)
        {
            var splitOutput = outputParam.Split(new char[] { ':' });

            if (splitOutput.Length == 1
                || !outputParam.EndsWith(".trx"))
                return "Error: /output parameter is in the incorrect format. Expected /output:<file name | directory and file name>. Execute /help for more information";

            return outputParam.Substring(8, outputParam.Length - 8);
        }
        private static List<string> ResolveTrxFilePaths(string trxParams, bool recursive)
        {
            List<string> paths = new List<string>();

            var splitTrx = trxParams.Split(new char[] { ':' });

            var searchOpts = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (splitTrx.Length == 1)
                return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.trx", searchOpts).ToList();

            var args = trxParams.Substring(5, trxParams.Length - 5).Split(new char[] { ',' }).ToList();

            foreach (var a in args)
            {
                bool isTrxFile = File.Exists(a) && a.EndsWith(".trx");
                bool isDir = Directory.Exists(a);

                //if (!isTrxFile && !isDir)
                //    return new List<string>
                //    {
                //        string.Format("Error: {0} is not a trx file or directory", a)
                //    };

                //if (isTrxFile)
                    paths.Add(a);

                if (isDir)
                    paths.AddRange(Directory.GetFiles(a, "*.trx", searchOpts).ToList());

            }

            return paths;
        }
    }
}
