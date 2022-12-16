using DcrConformanceChecker.Parsers.LogParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class TraceResult
{
    public readonly LogTrace Trace;
    public readonly bool IsSatisfied;
    public readonly string Message;

    public TraceResult(LogTrace trace, bool isSatisfied, string message)
    {
        Trace = trace;
        IsSatisfied = isSatisfied;
        Message = message;
    }
}