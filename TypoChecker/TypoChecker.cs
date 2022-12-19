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
    public static HashSet<String> getLogActivities(List<LogTrace> traces){
        HashSet<String> logActivities = traces.SelectMany(t => t.Events.Select(e => e.Activity)).ToHashSet<String>();
        return logActivities;
    }

    /// <summary>
    /// Returns tuples where no reversed pair exist in the list. This works by taking a list of tuples (list1) and then reversing pairs
    /// in a new list (list2), then picking elements each element in list1 that does not occur in list2. 
    /// </summary>
    /// <param name="tuples"></param>
    public static List<(String, String)> getDistinctTuple(List<(String,String)> tuples){
        return tuples.Where(p1 => (tuples.Select(p2 => (p2.Item2, p2.Item1))).All(p2 => p2 != p1)).ToList<(String, String)>();
    }


    /// <summary>
    /// Fuzzy matches DCRGraph activities and log activities in order to warn user that a typo in the DCRGraph might have accured.
    /// </summary>
    /// <param name="graphText"></param>
    /// <param name="traces"></param>
    /// <param name="threshold"></param>
    public static void TypoCheck(string graphText, List<LogTrace> traces, int threshold){
        var logActivities = getLogActivities(traces);
        var graph = DcrParser.ParseText(graphText);
        var pairs = new List<(String, String)>();
        foreach (Activity graphActiviy in graph.Activities){
            foreach(String logActivity in logActivities ){
                var ratio = Fuzz.Ratio(graphActiviy.Label, logActivity);
                if (ratio > threshold && ratio < 100){
                    pairs.Add((graphActiviy.Label,logActivity));
                }
            }
        }

        foreach ((String,String) pair in getDistinctTuple(pairs)) { 
            Console.WriteLine($"WARNING!: The graph activity <{pair.Item1}> look similar but not identical to log event <{pair.Item2}>. Was this intended?");
        }
    }
}
