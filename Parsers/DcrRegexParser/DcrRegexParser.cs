using DcrConformanceChecker.ConformanceChecker;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DcrConformanceChecker.Parsers.DcrRegexParser;

public class DcrRegexPaser
{
    // https://regex101.com/r/uDOcHo/1
    Regex regex = new Regex(@"(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+) *(-->\*|--><>|\*-->|-->%|-->\+) *(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+)|(([a-zA-Z0-9 ]+)\((1|0), *(1|0), *(1|0)\)|([a-zA-Z0-9 ]+)\(\))");
    
    private static string[] ReadFile(string path)
    {
        return File.ReadAllLines(path);
    }

    public void ParseLine(string line, DCRGraph graph)
    {
        MatchCollection matches = regex.Matches(line);

        foreach (Match match in matches)
        {   
            if (match.Groups[1].Length != 0 && match.Groups[3].Length != 0 && match.Groups[4].Length != 0 ){
                string left = match.Groups[1].Value;
                string arrow = match.Groups[3].Value;
                string right = match.Groups[4].Value;
                string[] src = left.Replace("(", "").Replace(")", "").Split(',');
                string[] trg = right.Replace("(", "").Replace(")", "").Split(',');
                for (int i = 0; i < src.Length; i++)
                {
                    src[i] = src[i].Trim();
                }
                for (int i = 0; i < trg.Length; i++)
                {
                    trg[i] = trg[i].Trim();
                }

                foreach (var to in trg)
                {
                    foreach(var from in src) {
                        switch (arrow)
                        {
                            case "-->*":
                                graph.AddCondition(from, to);
                                break;
                            case "--><>":
                                graph.AddMilestone(from, to);
                                break;
                            case "*-->":
                                graph.AddResponse(from, to);
                                break;
                            case "-->%":
                                graph.AddExclude(from, to);
                                break;
                            case "-->+":
                                graph.AddInclude(from, to);
                                break;
                        }
                    }
                }
            } else if (match.Groups[6].Length != 0) {
                string name = match.Groups[6].Value.Split('(')[0].Trim();
                string executed = match.Groups[7].Value.Trim();
                string included = match.Groups[8].Value.Trim();
                string pending = match.Groups[9].Value.Trim();
                
                if (executed.Length == 0 || included.Length == 0 || pending.Length == 0)
                {
                    graph.AddActivity(name);
                }
                else
                {
                    graph.AddActivity(name, Convert.ToBoolean(Convert.ToInt32(executed)), Convert.ToBoolean(Convert.ToInt32(included)), Convert.ToBoolean(Convert.ToInt32(pending)));
                }
            }
        }
    }
}
// match all
// (\((([a-zA-Z0-9 ]+), *)*([a-zA-Z0-9 ]+)\)|[a-zA-Z0-9 ]+) *(-->\*|--><>|\*-->|-->%|-->\+) *(\((([a-zA-Z0-9 ]+), *)*([a-zA-Z0-9 ]+)\)|[a-zA-Z0-9 ]+)|(([a-zA-Z0-9 ]+)\((1|0), *(1|0), *(1|0)\)|([a-zA-Z0-9 ]+)\(\))

// A -->* B
// [a-zA-Z0-9 ]+ -->\* [a-zA-Z0-9 ]+


// (A, B, C) -->* D
// \(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\) *(-->\*|--><>|\*-->|-->%|-->\+) *[a-zA-Z0-9 ]+

// A -->* (B, C, D)
// [a-zA-Z0-9 ]+ *(-->\*|--><>|\*-->|-->%|-->\+) *(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)) 

// (A, B, C) -->* (D, E, F)
// \(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\) *(-->\*|--><>|\*-->|-->%|-->\+) *\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)

// A()
// [a-zA-Z0-9 ]+\(\)

// A(1, 0, 1)
// [a-zA-Z0-9 ]+\((1|0), *(1|0), *(1|0)\)

// [a-zA-Z0-9 ]+\(\)|[a-zA-Z0-9 ]+\((1|0), *(1|0), *(1|0)\)