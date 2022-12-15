using DcrConformanceChecker.Parsers.LogParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class ConformanceCheck {

    private bool satisifed = true;

    private readonly DCRGraph graph;
    private readonly LogTrace trace;

    public ConformanceCheck(DCRGraph graph, LogTrace trace)
    {
        this.graph = graph;
        this.trace = trace;
    }

    public void RunCheck() {

        foreach (var e in trace.Events)
        {
            
            if (graph.HasActivity(e.Activity))
            {

                var a = graph.GetActivity(e.Activity);
                if (!a.IsEnabled())
                {
                    System.Console.WriteLine($"Event {e.Activity} is not enabled");
                    satisifed = false;
                    break;
                } else {
                    a.Execute();
                }

            }

        }

    }

    // return if its accepting
    public bool IsAccepting()
    {
        return graph.IsAccepting() && satisifed;
    }


}