namespace DcrConformanceChecker.LogParser;

public class LogParser
{

    private static string[] ReadFile(string path) {
        return File.ReadAllLines(path);
    }

    public static List<LogTrace> Parse(string path) {
        var lines = ReadFile(path).Skip(1);

        var data = new List<LogEvent>();

        foreach (var line in lines) {
            var values = line.Split(';');
            var logEvent = new LogEvent(values[0], values[2], values[4]);
            data.Add(logEvent);
        }

        var result = data.GroupBy(x => x.TraceId).Select(x => new LogTrace(x.ToList())).ToList();

        return result;
    }

}
