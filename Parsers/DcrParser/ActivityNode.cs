using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.Parsers.DcrParser;

public class ActivityNode : DcrParseNode
{
    public readonly string Label;
    public readonly bool Executed;
    public readonly bool Included;
    public readonly bool Pending;

    public ActivityNode(string label, bool executed = false, bool included = true, bool pending = false)
    {
        Label = label;
        Executed = executed;
        Included = included;
        Pending = pending;
    }

    // Evaluate the node by adding the activity to the DCR graph
    public override void Eval(DCRGraph graph)
    {
        graph.AddActivity(Label, Executed, Included, Pending);
    }
}
