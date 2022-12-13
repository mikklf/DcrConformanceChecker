using DcrConformanceChecker.ConformanceChecker;
using Sprache;

namespace DcrConformanceChecker.Parsers.DcrParser;

public class DcrPaser
{
    // Parser for a single name
    // Pattern: a-ZA-Z0-9_ and whitespace
    static readonly Parser<string> Name =
        (from first in Parse.LetterOrDigit.Once().Text()
         from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Or(Parse.WhiteSpace).Many().Text()
         select (first + rest).Trim()).Token();

    // Parser for multiple names
    // Pattern: (a-ZA-Z0-9_ and whitespace, ...)
    static readonly Parser<IEnumerable<string>> Names =
        from lpar in Parse.Char('(').Once().Token()
        from first in Name.Once()
        from subs in Parse.Char(',').Token().Then(_ => Name).Many()
        from rpar in Parse.Char(')').Once().Token()
        select first.Concat(subs);

    // Parser for a single or multiple names
    // Pattern: a-ZA-Z0-9_ and whitespace or (a-ZA-Z0-9_ and whitespace, ...)
    static readonly Parser<IEnumerable<string>> Activities =
        // Projicerer Name fra string til IEnumerable<string>
        Name.Select(n => new string[] { n }).Or(Names);


    // Operators
    // Pattern: -->* or --><> or *--> or -->+ or -->%
    static readonly Parser<RelationType> Response = Operator("-->*", RelationType.Condition);
    static readonly Parser<RelationType> Milestone = Operator("--><>", RelationType.Milestone);
    static readonly Parser<RelationType> Condition = Operator("*-->", RelationType.Response);
    static readonly Parser<RelationType> Include = Operator("-->+", RelationType.Include);
    static readonly Parser<RelationType> Exclude = Operator("-->%", RelationType.Exclude);

    // Helper function for parsing operators
    static Parser<RelationType> Operator(string op, RelationType opType)
    {
        return Parse.String(op).Token().Return(opType);
    }

    // Parser for a relations between one or more activities
    // Pattern: Activities -->* Activities
    static readonly Parser<DcrParseNode> Relation =
        from first in Activities
        from op in Response.Or(Milestone).Or(Condition).Or(Include).Or(Exclude)
        from rest in Activities
        select new RelationNode(first, op, rest);


    // Parser for a marking
    // Pattern: (1,1,1) or (0,0,0) or (1,0,0) or (0,1,0) or (0,0,1)
    static readonly Parser<(bool, bool, bool)> Marking =
        from lpar in Parse.Char('(').Once().Token()
        from first in Parse.Char('1').Once().Return(true).Or(Parse.Char('0').Once().Return(false))
        from spacer in Parse.Char(',').Once().Token()
        from second in Parse.Char('1').Once().Return(true).Or(Parse.Char('0').Once().Return(false))
        from spacer2 in Parse.Char(',').Once().Token()
        from third in Parse.Char('1').Once().Return(true).Or(Parse.Char('0').Once().Return(false))
        from rpar in Parse.Char(')').Once().Token()
        select (first, second, third);

    // Parser for an empty marking
    // Pattern: ()
    static readonly Parser<(bool, bool, bool)> EmptyMarking =
        from pars in Parse.String("()").Once().Token()
        select (true, false, false);

    // Parser for an activity
    // Pattern: Name(Marking)
    static readonly Parser<DcrParseNode> Activity =
        from name in Name
        from marking in Marking.Or(EmptyMarking)
        select new ActivityNode(name, marking.Item1, marking.Item2, marking.Item3);


    // Parser for a DCR graph
    // Pattern: Activity or Relation
    static readonly Parser<DcrParseNode> ParseDcr =
        Relation.Or(Activity);

    // Parse a string into a ParseNode
    static DcrParseNode ParseInput(string input) => ParseDcr.Parse(input);

    /// <summary>
    /// Parse a enumerable list of strings into a DCR graph
    /// </summary>
    public static DCRGraph ParseGraph(IEnumerable<string> lines)
    {
        var graph = new DCRGraph();

        foreach (var line in lines)
            DcrPaser.ParseInput(line).Eval(graph);

        return graph;
    }


}