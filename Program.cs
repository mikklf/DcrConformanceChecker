using DcrConformanceChecker.ConformanceChecker;

var graph = new DCRGraph();

graph.AddActivity("A");

graph.addCondition("A", "B");

graph.addExclude("B", "A");

var a = graph.GetActivity("A");
var b = graph.GetActivity("B");


a.Execute();
b.Execute();
a.Execute();

