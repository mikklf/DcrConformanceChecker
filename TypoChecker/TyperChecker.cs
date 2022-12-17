using FuzzySharp;
using DcrConformanceChecker.Parsers.LogParser;
using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.TypoChecker;
public class TypoChecker{

    public static HashSet<String> getActivities(List<LogTrace> traces){
        var logActivities = new HashSet<String>();
        foreach (LogTrace t in traces){
            foreach (LogEvent e in t.Events){
                logActivities.Add(e.Activity);
            }
        }
        return logActivities;
    }

    public static void TypoCheck(string graphText, List<LogTrace> traces, int threshold){
        var logActivities = getActivities(traces);
        var graph = DcrParser.ParseText(graphText);

        foreach (Activity graphActiviy in graph.Activities){
            foreach(String logActivity in logActivities ){
                var ratio = Fuzz.Ratio(graphActiviy.Label, logActivity);
                if (ratio > threshold && ratio < 100){
                    Console.WriteLine($"WARNING!: The graph activity <{graphActiviy.Label}> look similar but not identical to log event <{logActivity}>. Was this intended?");
                }
            }
        }
    }

}