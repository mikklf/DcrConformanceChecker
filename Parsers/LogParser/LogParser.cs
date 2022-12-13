namespace DcrConformanceChecker.Parsers.LogParser;

public class LogParser
{

    private static string[] ReadFile(string path)
    {
        return File.ReadAllLines(path);
    }

    /// <summary>
    /// Parse a text file into a list of LogTraces
    /// </summary>
    public static List<LogTrace> ParseFile(string path)
    {
        var lines = ReadFile(path).Skip(1);
        var data = new List<LogEvent>();

        foreach (var line in lines)
        {
            var values = line.Split(';');
            
            // Extract the ID, Title and Date from the line
            data.Add(new LogEvent(values[0], values[2], values[4]));
        }

        // Group lines by TraceId
        // Create a LogTrace for each group
        // Add it to a list of LogTraces
        return data.GroupBy(x => x.TraceId).Select(x => new LogTrace(x.ToList())).ToList();
    }

}
