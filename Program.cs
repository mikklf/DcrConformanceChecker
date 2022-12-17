using DcrConformanceChecker.ConformanceChecker;
using DcrConformanceChecker.Parsers.LogParser;
using DcrConformanceChecker.TypoChecker;

if (args.Length != 2) 
{
    System.Console.WriteLine("Wrong arguments. Usage: dotnet run <dcr file> <log file>");
    return;
}

if (!File.Exists(args[0]))
{
    System.Console.WriteLine($"{args[0]} does not exist");
    return;
}

if (!File.Exists(args[1]))
{
    System.Console.WriteLine($"{args[1]} does not exist");
    return;
}

var textLines = File.ReadAllText(args[0]);
var traces = LogParser.ParseFile(args[1]);

var checker = new ConformanceChecker(textLines, traces);
var result = checker.RunConformanceCheck();

foreach (var traceResult in result.UnsatisfiedTraces)
{
    System.Console.WriteLine();
    System.Console.WriteLine($"Trace Id: {traceResult.Trace.TraceId} is not satisfied: {traceResult.Message}");
    traceResult.Trace.PrintTrace();
    System.Console.WriteLine();
}

Console.WriteLine($"Satisfied: {result.SatisfiedTraces.Count} / {result.TotalTraceCount}");
Console.WriteLine($"Unsatisfied: {result.UnsatisfiedTraces.Count} / {result.TotalTraceCount}");

TypoChecker.TypoCheck(textLines, traces, 80);