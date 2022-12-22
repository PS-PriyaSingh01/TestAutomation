using System;
using System.Collections.Generic;
using System.Linq;

namespace TRXFileM
{
    public static class TestRunMerger
    {
        public static TestRun MergeTRXsAndSave(List<string> trxFiles, string outputFile)
        {
            Console.WriteLine("Deserializing trx files:");
            List<TestRun> runs = new List<TestRun>();
            foreach (var trx in trxFiles)
            {
                Console.WriteLine(trx);
                runs.Add(TRXSerializationUtils.DeserializeTRX(trx));
            }

            Console.WriteLine("Combining deserialized trx files...");
            var combinedTestRun = MergeTestRuns(runs);

            Console.WriteLine("Saving result...");
            var savedFile = TRXSerializationUtils.SerializeAndSaveTestRun(combinedTestRun, outputFile);

            Console.WriteLine("Operation completed:");
            Console.WriteLine("\tCombined trx files: " + trxFiles.Count);
            Console.WriteLine("\tResult trx file: " + savedFile);

            return combinedTestRun;
        }

        private static TestRun MergeTestRuns(List<TestRun> testRuns)
        {
            string name = testRuns[0].Name;
            string runUser = testRuns[0].RunUser;

            string startString = "";
            DateTime startDate = DateTime.MaxValue;

            string endString = "";
            DateTime endDate = DateTime.MinValue;

            List<UnitTestResult> allResults = new List<UnitTestResult>();
            List<UnitTest> allTestDefinitions = new List<UnitTest>();
            List<TestEntry> allTestEntries = new List<TestEntry>();
            List<TestList> allTestLists = new List<TestList>();


            var resultSummary = new ResultSummary
            {
                Counters = new Counters(),
                RunInfos = new List<RunInfo>(),
            };
            bool resultSummaryPassed = true;

            for (int h = 0; h < testRuns.Count; h++)
            {
                if (allResults.Count > 0)
                {
                    for (int i = 0; i < allResults.Count; i++)
                    {
                        for (int j = 0; j < testRuns[h].Results.Count; j++)
                        {
                            if (allResults[i].TestId == testRuns[h].Results[j].TestId)
                            {
                                testRuns[h].Results.Remove(testRuns[h].Results[j]);
                                break;
                            }
                        }
                    }
                }
                allResults = allResults.Concat(testRuns[h].Results).ToList();
                
                if (allTestDefinitions.Count > 0)
                {
                    for (int i = 0; i < allTestDefinitions.Count; i++)
                    {
                        for (int j = 0; j < testRuns[h].TestDefinitions.Count; j++)
                        {
                            if (allTestDefinitions[i].Id == testRuns[h].TestDefinitions[j].Id)
                            {
                                testRuns[h].TestDefinitions.Remove(testRuns[h].TestDefinitions[j]);
                                break;
                            }
                        }
                    }
                }
                allTestDefinitions = allTestDefinitions.Concat(testRuns[h].TestDefinitions).ToList();
                
                if (allTestEntries.Count > 0)
                {
                    for (int i = 0; i < allTestEntries.Count; i++)
                    {
                        for (int j = 0; j < testRuns[h].TestEntries.Count; j++)
                        {
                            if (allTestEntries[i].TestId == testRuns[h].TestEntries[j].TestId)
                            {
                                testRuns[h].TestEntries.Remove(testRuns[h].TestEntries[j]);
                                break;
                            }
                        }
                    }
                }
                allTestEntries = allTestEntries.Concat(testRuns[h].TestEntries).ToList();
                allTestLists = allTestLists.Concat(testRuns[h].TestLists).ToList();

                DateTime currStart = DateTime.Parse(testRuns[h].Times.Start);
                if (currStart < startDate)
                {
                    startDate = currStart;
                    startString = testRuns[h].Times.Start;
                }

                DateTime currEnd = DateTime.Parse(testRuns[h].Times.Finish);
                if (currEnd > endDate)
                {
                    endDate = currEnd;
                    endString = testRuns[h].Times.Finish;
                }

                if (h == 0)
                    resultSummaryPassed &= testRuns[h].ResultSummary.Outcome == "Passed";
                resultSummary.RunInfos = resultSummary.RunInfos.Concat(testRuns[h].ResultSummary.RunInfos).ToList();
                resultSummary.Counters.Aborted += testRuns[h].ResultSummary.Counters.Aborted;
                resultSummary.Counters.Completed += testRuns[h].ResultSummary.Counters.Completed;
                resultSummary.Counters.Disconnected += testRuns[h].ResultSummary.Counters.Disconnected;
                resultSummary.Counters.Еxecuted = testRuns[h].ResultSummary.Counters.Еxecuted;
                if (h == 0)
                    resultSummary.Counters.Failed = testRuns[h].ResultSummary.Counters.Failed;
                resultSummary.Counters.Inconclusive += testRuns[h].ResultSummary.Counters.Inconclusive;
                resultSummary.Counters.InProgress += testRuns[h].ResultSummary.Counters.InProgress;
                resultSummary.Counters.NotExecuted += testRuns[h].ResultSummary.Counters.NotExecuted;
                resultSummary.Counters.NotRunnable += testRuns[h].ResultSummary.Counters.NotRunnable;
                resultSummary.Counters.Passed += testRuns[h].ResultSummary.Counters.Passed;
                resultSummary.Counters.PassedButRunAborted += testRuns[h].ResultSummary.Counters.PassedButRunAborted;
                resultSummary.Counters.Pending += testRuns[h].ResultSummary.Counters.Pending;
                resultSummary.Counters.Timeout += testRuns[h].ResultSummary.Counters.Timeout;
                resultSummary.Counters.Total = testRuns[h].ResultSummary.Counters.Total;
                resultSummary.Counters.Warning += testRuns[h].ResultSummary.Counters.Warning;

            }

            resultSummary.Outcome = resultSummaryPassed ? "Passed" : "Failed";

            return new TestRun
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                RunUser = runUser,
                Times = new Times
                {
                    Start = startString,
                    Queuing = startString,
                    Creation = startString,
                    Finish = endString,
                },
                Results = allResults,
                TestDefinitions = allTestDefinitions,
                TestEntries = allTestEntries,
                TestLists = allTestLists,
                ResultSummary = resultSummary,
            };
        }

    }
}
