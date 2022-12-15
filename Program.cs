using DcrConformanceChecker.ConformanceChecker;
using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.Parsers.LogParser;

// var a = LogParser.ParseFile(@"C:\Users\mikke\Downloads\log.csv");

// var fr = a.First();

// fr.PrintTrace();



var lines = """
A(0,0,0)
B(0,1,1)
A -->* B
B *--> A
C -->% A
D -->+ A
D -->* B
A --><> (B, D)
""";

var graph = DcrParser.ParseText(lines);

Console.WriteLine(graph.Activities.Count());

foreach (var x in graph.Activities)
{
    Console.WriteLine(x.ToString());
    System.Console.WriteLine(x.ConditionIn.Count());
    System.Console.WriteLine(x.ResponseOut.Count());
    System.Console.WriteLine(x.MilestoneIn.Count());
}






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