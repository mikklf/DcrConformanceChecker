using DcrConformanceChecker.ConformanceChecker;

namespace DcrConformanceChecker.Parsers.DcrParser;

public abstract class DcrParseNode
{

    public abstract void Eval(DCRGraph graph);

}