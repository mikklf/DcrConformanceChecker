namespace DcrConformanceChecker.LogParser;

public class LogTrace {

    public readonly List<LogEvent> Trace;

    public LogTrace(List<LogEvent> trace)
    {
        Trace = trace;

        // Sort Trace by timestamp ascending
        Trace = Trace.OrderBy(x => x.Timestamp).ToList();
    }
}