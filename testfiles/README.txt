=== Prerequisite ===
The project is built on .NET 7.0 which is required to build and run the project.
.NET 7.0 can be downloaded from: https://dotnet.microsoft.com/en-us/download/dotnet/7.0

The project use two addtional NuGet Packages to installs these navigate to the folder containing the .csproj file
and run the followings commands:
 dotnet restore
 dotnet build

=== Running the program ===
To run the program navigate to the folder containing the .csproj file and run the followings commands:
 dotnet run <dcr path> <log path>

Example: dotnet run pattern2.txt log.csv

This should give the following output:
 Satisfied: 305 / 594
 Unsatisfied: 289 / 594

=== Addtional terminal output ===
If you want more information use lite or full as third argument
 Lite lists unsatisfied traces id and the reason why
 Full also includes the full trace for both satisfied and unsatisfied traces.

Example: dotnet run pattern1.txt log.csv lite
Example: dotnet run pattern1.txt log.csv full