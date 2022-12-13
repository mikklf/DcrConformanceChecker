namespace DcrConformanceChecker.Parsers.LogParser;

public class LogEvent
{

    public readonly string TraceId;
    public readonly string Activity;
    public readonly DateTime Timestamp;

    public LogEvent(string traceId, string activity, string timestamp)
    {
        TraceId = traceId;
        Activity = activity;
        Timestamp = DateTime.Parse(timestamp);
    }
}
