namespace DcrConformanceChecker.Parsers.LogParser;

public class LogTrace
{

    public readonly List<LogEvent> Events;
    public string TraceId => Events.First().TraceId;

    public LogTrace(List<LogEvent> events)
    {
        Events = events;

        // Sort Trace by timestamp ascending
        Events = Events.OrderBy(x => x.Timestamp).ToList();
    }

    public void PrintTrace()
    {
        var trace_str = String.Join(", ", Events.Select(x => x.Activity));
        Console.WriteLine($"{TraceId} = <{trace_str}>");
    }
}
