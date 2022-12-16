using DcrConformanceChecker.ConformanceChecker;
using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.Parsers.LogParser;

var lines = """
Approve changed account(0,0,0)
Reject *--> (Change phase to Abort, Applicant informed)
First payment -->% First payment
Undo payment -->+ First payment
Change phase to Payout *--> First payment
Account number changed *--> Approve changed account
Account number changed -->+ Approve changed account
Approve changed account --><> First payment
First payment --><> Change phase to End Report
""";

var lines2 = """
Fill out application -->* Change phase to Review
Fill out application -->* Architect Review
Fill out application -->* Review
Fill out application -->* Lawyer Review
Fill out application -->* Register Decision
Fill out application -->* Change phase to Board meeting
Fill out application -->* Round ends
Fill out application -->* Round approved
Fill out application -->* Inform application of board review
Fill out application -->* Reject
Fill out application -->* Applicant informed
Fill out application -->* Change phase to Abort
Fill out application -->* Screening reject
Fill out application -->* Screen application
Fill out application -->* Execute pre-decision
Fill out application -->* Approve
Fill out application -->* Change phase to Preparation
Fill out application -->* Inform applicant of approval
Fill out application -->* Applicant justifies relevance
Fill out application -->* Change Phase to Payout
Fill out application -->* First payment
Fill out application -->* Payment completed
Fill out application -->* Change Phase to End Report
Fill out application -->* Account number changed
Fill out application -->* Receive end report
Fill out application -->* Change phase to Complete
Fill out application -->* Execute abandon
Fill out application -->* Change phase to Abandon
""";

var traces = LogParser.ParseFile(@"C:\Users\mikke\Desktop\DcrConformanceChecker\log.csv");

var result = new ConformanceChecker(lines, traces).RunConformanceCheck();
foreach (var traceResult in result.UnsatisfiedTraces)
{
    System.Console.WriteLine();
    System.Console.WriteLine($"Trace Id: {traceResult.Trace.TraceId} is not satisfied: {traceResult.Message}");
    traceResult.Trace.PrintTrace();
    System.Console.WriteLine();
}

Console.WriteLine($"Satisfied: {result.SatisfiedTraces.Count} / {result.TotalTraceCount}");
Console.WriteLine($"Unsatisfied: {result.UnsatisfiedTraces.Count} / {result.TotalTraceCount}");