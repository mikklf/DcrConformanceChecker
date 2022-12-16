using DcrConformanceChecker.Parsers.LogParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class ConformanceResult
{
    
    public readonly List<TraceResult> SatisfiedTraces;
    public readonly List<TraceResult> UnsatisfiedTraces;

    /// <summary>
    /// Returns the total number of traces that have been checked
    /// </summary>
    public int TotalTraceCount => SatisfiedTraces.Count + UnsatisfiedTraces.Count;

    public ConformanceResult()
    {
        SatisfiedTraces = new List<TraceResult>();
        UnsatisfiedTraces = new List<TraceResult>();
    }
}