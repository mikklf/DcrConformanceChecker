using System.Runtime.CompilerServices;
using DcrConformanceChecker.ConformanceChecker;
using DcrConformanceChecker.Parsers.LogParser;
using DcrConformanceChecker.TypoChecker;

//////////////////////
// Input validation //
//////////////////////
if (args.Length < 2)
    Abort("Usage: dotnet run <dcr file> <log file>");

if (!File.Exists(args[0]))
    Abort($"{args[0]} does not exist");

if (!File.Exists(args[1]))
    Abort($"{args[1]} does not exist");

//////////////////////////
// Conformance checking //
//////////////////////////
var textLines = File.ReadAllText(args[0]);
var traces = LogParser.ParseFile(args[1]);

var checker = new ConformanceChecker(textLines, traces);
var result = checker.RunConformanceCheck();

////////////////////
// Output results //
////////////////////

// Output addtional information if specified
if (args.Length == 3) {
    if (args[2] == "Lite" || args[2] == "lite" ||  args[2] == "L" || args[2] == "l")
        PrintAddtionalOutput(result, AddtionalOutputType.Lite);
    else if (args[2] == "Full" || args[2] == "full" ||  args[2] == "F" || args[2] == "f")
        PrintAddtionalOutput(result, AddtionalOutputType.Full);
}

// Output the main result
Console.WriteLine();
Console.WriteLine($"Satisfied: {result.SatisfiedTraces.Count} / {result.TotalTraceCount}");
Console.WriteLine($"Unsatisfied: {result.UnsatisfiedTraces.Count} / {result.TotalTraceCount}");

// Print potential typo warnings
TypoChecker.TypoCheck(textLines, traces, 80);

//////////////////////
// Helper functions //
//////////////////////
void Abort(string message)
{
    Console.WriteLine(message);
    Environment.Exit(1);
}

void PrintAddtionalOutput(ConformanceResult data, AddtionalOutputType type) {
    
    if (type == AddtionalOutputType.Lite)
    {
        Console.WriteLine("\nUnsatisfied Traces:");
        foreach (TraceResult tResult in result.UnsatisfiedTraces){
            Console.WriteLine($"{tResult.Trace.TraceId}: {tResult.Message}");
        }
    } 
    else 
    {
        Console.WriteLine("\nSatisfied Traces:\n");
        foreach (TraceResult tResult in result.SatisfiedTraces) {
            System.Console.WriteLine();
            System.Console.WriteLine($"Trace Id: {tResult.Trace.TraceId} is satisfied: {tResult.Message}");
            tResult.Trace.PrintTrace();
            System.Console.WriteLine();
        }

        Console.WriteLine("\nUnsatisfied Traces:\n");
        foreach (TraceResult traceResult in result.UnsatisfiedTraces)
        {
            System.Console.WriteLine();
            System.Console.WriteLine($"Trace Id: {traceResult.Trace.TraceId} is not satisfied: {traceResult.Message}");
            traceResult.Trace.PrintTrace();
            System.Console.WriteLine();
        }
    }

}

enum AddtionalOutputType { Lite, Full }