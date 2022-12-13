using DcrConformanceChecker.ConformanceChecker;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DcrConformanceChecker.Parsers.DcrRegexParser;

public class DcrRegexPaser
{

    Regex regexArrow = new Regex(@"(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+) *(-->\*|--><>|\*-->|-->%|-->\+) *(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+)");
    Regex regexMarking = new Regex(@"[a-zA-Z0-9 ]+\((1|0), *(1|0), *(1|0)\)|[a-zA-Z0-9 ]+\(\)");

    private static string[] ReadFile(string path)
    {
        return File.ReadAllLines(path);
    }

    public void ParseLine(string line, DCRGraph graph)
    {
        MatchCollection matchesArrow = regexArrow.Matches(line);
        MatchCollection matchesMarking = regexMarking.Matches(line);

        if (matchesArrow.Count != 0)
        {
            foreach (Match match in matchesArrow)
            {
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

                foreach (var from in src)
                {
                    foreach (var to in trg)
                    {
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
            }
        }
        else if (matchesMarking.Count != 0)
        {
            string name = matchesMarking[0].Groups[0].Value.Split("(")[0];
            string executed = matchesMarking[0].Groups[1].Value;
            string included = matchesMarking[0].Groups[2].Value;
            string pending = matchesMarking[0].Groups[3].Value;

            if (executed.Length == 0 && included.Length == 0 && pending.Length == 0)
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
// match all
// (\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+) *(-->\*|--><>|\*-->|-->%|-->\+) *(\(([a-zA-Z0-9 ]+, *)*[a-zA-Z0-9 ]+\)|[a-zA-Z0-9 ]+)

// A -->* B
// [a-zA-Z0-9 ]+ -->\* [a-zA-Z]+


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