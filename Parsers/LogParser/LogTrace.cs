namespace DcrConformanceChecker.Parsers.LogParser;

public class LogTrace
{

    public readonly List<LogEvent> Trace;
    public string TraceID => Trace.First().TraceId;

    public LogTrace(List<LogEvent> trace)
    {
        Trace = trace;

        // Sort Trace by timestamp ascending
        Trace = Trace.OrderBy(x => x.Timestamp).ToList();
    }

    public void PrintTrace()
    {

        Console.Write($"{TraceID} = <");

        foreach (var logEvent in Trace)
            Console.Write(logEvent.Activity + ", ");

        Console.Write("> \n");
    }
}
