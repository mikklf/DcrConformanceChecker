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
        var trace_str = String.Join(", ", Trace.Select(x => x.Activity));
        Console.Write($"{TraceID} = <{trace_str}>");
    }
}
