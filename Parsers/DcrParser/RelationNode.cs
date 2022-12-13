using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.Parsers.DcrParser;

public class RelationNode : DcrParseNode
{
    public IEnumerable<string> Source { get; private set; }
    public RelationType RelationType { get; private set; }
    public IEnumerable<string> Target { get; private set; }

    public RelationNode(IEnumerable<string> from, RelationType type, IEnumerable<string> to)
    {
        Source = from;
        RelationType = type;
        Target = to;
    }

    // Evaluate the node by adding the relation to the DCR graph, this also adds the activities if they are not already added
    public override void Eval(DCRGraph graph)
    {
        // create switch case for each type
        foreach (var from in Source)
        {
            foreach (var to in Target)
            {

                switch (RelationType)
                {
                    case RelationType.Response:
                        graph.AddResponse(from, to);
                        break;
                    case RelationType.Milestone:
                        graph.AddMilestone(from, to);
                        break;
                    case RelationType.Condition:
                        graph.AddCondition(from, to);
                        break;
                    case RelationType.Include:
                        graph.AddInclude(from, to);
                        break;
                    case RelationType.Exclude:
                        graph.AddExclude(from, to);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
