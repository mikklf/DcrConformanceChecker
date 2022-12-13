using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.Parsers.DcrParser;

public class ActivityNode : DcrParseNode
{
    public readonly string Label;
    public readonly bool Included;
    public readonly bool Executed;
    public readonly bool Pending;

    public ActivityNode(string label, bool included = true, bool executed = false, bool pending = false)
    {
        Label = label;
        Included = included;
        Executed = executed;
        Pending = pending;
    }

    // Evaluate the node by adding the activity to the DCR graph
    public override void Eval(DCRGraph graph)
    {
        graph.AddActivity(Label, Included, Executed, Pending);
    }
}
