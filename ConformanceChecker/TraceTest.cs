using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.Parsers.LogParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class TraceTest {
    private readonly DCRGraph graph;
    private readonly LogTrace trace;

    public TraceTest(DCRGraph graph, LogTrace trace)
    {
        this.graph = graph;
        this.trace = trace;
    }

    /// <summary>
    /// Run the trace check on the given log trace and graph <br/>
    /// Returns a TraceResult object containing the result of the trace check
    /// </summary>
    public TraceResult RunTraceCheck() {

        // Run every event in the trace
        foreach (var e in trace.Events)
        {
            // If it does not exist in the graph, skip it
            if (graph.HasActivity(e.Activity))
            {
                var a = graph.GetActivity(e.Activity);

                // If the activity is not enabled, the trace is unsatisfied.
                // and we return a TraceResult with a message that contains the event that caused the unsatisfaction
                // otherwise we execute the activity and change the state of the graph
                if (!a.IsEnabled())
                {
                    return new TraceResult(trace, false, $"Event {e.Activity} is not enabled");
                } else {
                    a.Execute();
                }
            } 
        }

        // If the graph is accepting after running each event, the trace is satisfied
        // otherwise we return a TraceResult with a message that contains the pending activities
        if (graph.IsAccepting())
        {
            return new TraceResult(trace, true, "Trace is satisfied");
        } else {
            var pendingsStr = String.Join(", ", graph.GetPendingActivities().Select(a => a.Label));
            return new TraceResult(trace, false, $"Trace has pending activities: {pendingsStr}");
        }
    }
}