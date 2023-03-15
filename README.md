# DCR Conformance Checker
This repo contains a simple conformance checker for Dynamic reponse graphs ([DCR](https://pure.itu.dk/en/publications/hierarchical-declarative-modelling-with-refinement-and-sub-proces)) built with .NET 7

The project consist of a conformance checker, a simple parser for DCR graphs and a typo checker to ensure events used in graph are consistent with events in the log file.

The Parser accepts the following syntax

`A(0,1,0)`   Defines an event A that is not executed, is included and not pending  
`B(1,0,1)`   Defines an event B that is executed, not included and is pending  
`A -->* B`   B must happen eventually after executing (B becomes pending)  
`A --<> B`   B can not happen as long as A is pending  
`A *--> B`   B can not happen before A has been executed  
`A -->+ B`   Executing A includes B  
`A -->% B`   Executing A excludes B

Events can also be grupped like `(A,B,C) -->* D` which means that executing A, B or C makes D pending. Defining events and their markings is not required, if an undefined event is used in a rule it will have default marking `(0,1,0)` that is not executed, included and not pending.

# Prerequisite
The project is built on .NET 7.0 which is required to build and run the project. .NET 7.0 can be downloaded from: https://dotnet.microsoft.com/en-us/download/dotnet/7.0

The project use two addtional NuGet Packages to install these navigate to the folder containing the .csproj file and run the `dotnet restore` command.

# Running the program
To run the program navigate to the folder containing the .csproj file and run `dotnet run <dcr path> <log path>` where `<dcr path>` is a file containing the graph and `<log path>` is the log file to run conformance checking against. Check the `testfiles` folder for example patterns and log file.

Example: `dotnet run pattern2.txt log.csv`

This should give the following output:
```
 Satisfied: 305 / 594
 Unsatisfied: 289 / 594
```

# Addtional terminal output
If you want more information use `lite` or `full` as third argument  
* `lite` lists unsatisfied traces id and the reason why  
* `full` also includes the full trace for both satisfied and unsatisfied traces.  

Example: `dotnet run pattern1.txt log.csv lite`  
Example: `dotnet run pattern1.txt log.csv full`