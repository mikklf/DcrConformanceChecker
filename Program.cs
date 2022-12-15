using DcrConformanceChecker.ConformanceChecker;
using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.Parsers.LogParser;

var traces = LogParser.ParseFile(@"C:\Users\mikke\Desktop\DcrConformanceChecker\log.csv");

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

var accepting = 0;
var total = traces.Count;

foreach (var trace in traces)
{
    var graph = DcrParser.ParseText(lines2);

    var checker = new ConformanceCheck(graph, trace);

    checker.RunCheck();

    if (checker.IsAccepting())
    {
        accepting++;
    } else {
        trace.PrintTrace();
    }
}

Console.WriteLine($"Accepting: {accepting} / {total}");







// Regex regexArrow = new Regex(@"(\(([a-zA-Z ]+, *)*[a-zA-Z ]+\)|[a-zA-Z ]+) *(-->\*|--><>|\*-->|-->%|-->\+) *(\(([a-zA-Z ]+, *)*[a-zA-Z ]+\)|[a-zA-Z ]+)");

// string input = "Fill out app -->* (B, C, D)";

// MatchCollection matches = regexArrow.Matches(input);

// Console.WriteLine("{0} matches found in: {1}", matches.Count, input);

// Console.WriteLine(matches[0].Groups[3].Value);

// foreach (Match match in matches)
// {
//     // Get the left-hand side and right-hand side of the arrow
//     string left = match.Groups[1].Value;
//     string arrow = match.Groups[3].Value;
//     string right = match.Groups[4].Value;
//     Console.WriteLine($"Left: {left}, Arrow: {arrow}, Right: {right}");
//     var test = left.Replace("(", "").Replace(")", "").Split(',');
//     var test2 = right.Replace("(", "").Replace(")", "").Split(',');
//     for (int i = 0; i < test.Length; i++) 
//     {
//         test[i] = test[i].Trim();
//     }
//     for (int i = 0; i < test2.Length; i++) 
//     {
//         test2[i] = test2[i].Trim();
//     }
//     Console.WriteLine(test[0]);
//     Console.WriteLine($"{test2[0]} {test2[1]} {test2[2]}");
// }

// // Fill out app (1,0,0)

// Regex regexMarking = new Regex(@"[a-zA-Z ]+\((1|0), *(1|0), *(1|0)\)|[a-zA-Z ]+\(\)");

// string input2 = "Fill out app(1, 0, 0)";

// MatchCollection matchesMarking = regexMarking.Matches(input2);

// Console.WriteLine("{0} matches found in: {1}", matchesMarking.Count, input2);

// Console.WriteLine($"{matchesMarking[0].Groups[0].Value.Split("(")[0]}");
// Console.WriteLine($"{matchesMarking[0].Groups[1].Value.Length}");
// Console.WriteLine($"{matchesMarking[0].Groups[2].Value.Length}");
// Console.WriteLine($"{matchesMarking[0].Groups[3].Value.Length}");
// Console.WriteLine($"{Convert.ToBoolean(Convert.ToInt32("1"))}");