using DcrConformanceChecker.Parsers.DcrParser;
using DcrConformanceChecker.Parsers.LogParser;

namespace DcrConformanceChecker.ConformanceChecker;

public class ConformanceChecker {

        private readonly string graphText;
        private readonly List<LogTrace> trace;

        public ConformanceChecker(string graphText, List<LogTrace> traces)
        {
            this.graphText = graphText;
            this.trace = traces;
        }

        /// <summary>
        /// Run the conformance check on the given log traces and graph <br/>
        /// Returns a ConformanceResult object containing the result of the conformance check
        /// </summary>
        public ConformanceResult RunConformanceCheck() 
        {
            ConformanceResult conformanceResult = new ConformanceResult();

            // Run trace check on each trace
            foreach (var t in trace)
            {
                // First we parse the graph text into a DCRGraph object.
                // This ensures we get a fresh graph for each trace
                var graph = DcrParser.ParseText(this.graphText);
                TraceTest checker = new TraceTest(graph, t);


                TraceResult traceResult = checker.RunTraceCheck();

                // Add the trace result to the conformance result depending on if it is satisfied or not
                if (traceResult.IsSatisfied)
                {
                    conformanceResult.SatisfiedTraces.Add(traceResult);
                } else {
                    conformanceResult.UnsatisfiedTraces.Add(traceResult);
                }
            }

            return conformanceResult;
        }
}