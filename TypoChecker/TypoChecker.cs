using FuzzySharp;
using DcrConformanceChecker.Parsers.LogParser;
using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.TypoChecker;
public class TypoChecker{

    /// <summary>
    /// Selects all Activities from a List of Logtraces. 
    /// Since the output is a HashSet the output elements should be distinct, meaning that there are no identical elements.
    /// </summary>
    /// <param name="traces"></param>
    /// <returns></returns>
    public static List<String> getLogActivities(List<LogTrace> traces){
        return traces.SelectMany(t => t.Events.Select(e => e.Activity)).Distinct().ToList();
    }

    /// <summary>
    /// Parses graphtext and converts DCR graph to list of distinct activity labels.
    /// <param name="graphText"></param>
    /// <returns></returns>
    public static List<String> getDCRActivities(string graphText){
        return DcrParser.ParseText(graphText).Activities.Select(a => a.Label).Distinct().ToList();
    }

    /// <summary>
    /// Eliminates Exact Matches between graph list and log list activities from graph list.
    /// </summary>
    /// <param name="logActivities"></param>
    /// <param name="graphActivities"></param>
    /// <returns></returns>
    public static List<String> eliminateExactMatch (List<String> logActivities, List<String> graphActivities){
        return graphActivities.Where(a1 => logActivities.All(a2 => a1 != a2)).ToList();
    }

    /// <summary>
    /// Fuzzy matches DCRGraph activities and log activities in order to warn user that a typo in the DCRGraph might have occured.
    /// </summary>
    /// <param name="graphText"></param>
    /// <param name="traces"></param>
    /// <param name="threshold"></param>
    public static void TypoCheck(string graphText, List<LogTrace> traces, int threshold){
        var logActivities = getLogActivities(traces);
        var graphActivities = eliminateExactMatch(logActivities, getDCRActivities(graphText));
        var pairs = new List<(String, String)>();

        foreach (String graphActiviy in (graphActivities)){
            foreach(String logActivity in logActivities ){
                if (Fuzz.Ratio(graphActiviy, logActivity) > threshold){
                    pairs.Add((graphActiviy, logActivity));
                }
            }
        }

        foreach ((String,String) pair in pairs) { 
            Console.WriteLine($"WARNING!: The graph activity <{pair.Item1}> look similar but not identical to log event <{pair.Item2}>. Was this intended?");
        }
    }
}
